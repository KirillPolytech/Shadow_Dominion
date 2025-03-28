using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Multiplayer.Structs;
using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine
    {
        protected new readonly List<PlayerState> _states = new();
        private readonly MirrorPlayer _mirrorPlayer;
        private readonly Transform _ragdollRoot;
        private readonly PlayerAnimation _playerAnimation;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly BoneController[] _boneControllers;
        private readonly IInputHandler _inputHandler;
        private readonly PlayerSettings _playerSettings;

        public new PlayerState CurrentState { get; protected set; }

        public Action<PlayerStateMessage> OnStateChanged;

        private string _lastState;

        public PlayerStateMachine(
            MirrorPlayer mirrorPlayer,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneControllers,
            CoroutineExecuter coroutineExecuter,
            PlayerMovement playerMovement,
            IInputHandler inputHandler,
            AnimationClip standUpFaceUp,
            AnimationClip standUpFaceDown,
            WindowsController windowsController,
            Ak47 ak47,
            PlayerSettings playerSettings)
        {
            _mirrorPlayer = mirrorPlayer;
            _ragdollRoot = ragdollRoot;
            _playerAnimation = playerAnimation;
            _coroutineExecuter = coroutineExecuter;
            _boneControllers = boneControllers;
            _inputHandler = inputHandler;
            _playerSettings = playerSettings;
            
            DefaultState defaultState = new DefaultState(playerAnimation, playerMovement, inputHandler,
                windowsController, boneControllers, rootRig, cameraLook, ak47);
            InActiveState inActiveState = new InActiveState(mirrorPlayer, playerAnimation);
            PauseMenuState pauseMenuState = new PauseMenuState(windowsController, playerAnimation);
            DeathState deathState = new DeathState(playerAnimation, boneControllers, cameraLook);
            
            StandUpFaceUpState standUpFaceUpState =
                new StandUpFaceUpState(rootRig, playerAnimation,
                    cameraLook, coroutineExecuter, standUpFaceUp.length / 2, boneControllers, MoveAnimToRagdoll, this);
            StandUpFaceDownState standUpFaceDownState =
                new StandUpFaceDownState(rootRig, playerAnimation,
                    cameraLook, coroutineExecuter, standUpFaceDown.length / 2, boneControllers, MoveAnimToRagdoll, this, ak47);
            RagdollState ragdollState =
                new RagdollState(playerAnimation, cameraLook, rootRig, boneControllers, ragdollRoot,
                    this, mirrorPlayer, ak47, coroutineExecuter, playerSettings);

            _states.Add(standUpFaceUpState);
            _states.Add(standUpFaceDownState);
            _states.Add(ragdollState);
            _states.Add(defaultState);
            _states.Add(inActiveState);
            _states.Add(deathState);
            _states.Add(pauseMenuState);

            _inputHandler.OnInputUpdate += HandleInput;
        }

        ~PlayerStateMachine()
        {
            _inputHandler.OnInputUpdate -= HandleInput;
        }

        private void HandleInput(InputData inputData)
        {
            if (!inputData.ESC)
                return;

            if (CurrentState.GetType() != typeof(PauseMenuState))
            {
                _lastState = CurrentState.GetType().ToString();
                SetState<PauseMenuState>();
                return;
            }

            SetState(StringToState(_lastState));
            _lastState = string.Empty;
        }

        public override void SetState<T>()
        {
            PlayerState state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state || (CurrentState != null && !CurrentState.CanExit()))
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            OnStateChanged?.Invoke(new PlayerStateMessage(CurrentState.GetType().ToString()));

            Debug.Log($"Current player state: {CurrentState.GetType()}, Time: {Time.time}");
        }

        public void SetState(PlayerState state)
        {
            if (CurrentState == state || (CurrentState != null && !CurrentState.CanExit()))
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            OnStateChanged?.Invoke(new PlayerStateMessage(CurrentState.GetType().ToString()));

            Debug.Log($"Current player state: {CurrentState.GetType()}, Time: {Time.time}");
        }

        public void SetState(string stateName)
        {
            PlayerState state = StringToState(stateName);

            if (CurrentState == state || (CurrentState != null && !CurrentState.CanExit()))
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        private PlayerState StringToState(string state)
            => _states.First(x => x.GetType().ToString() == state);


        #region Coroutines

        private IEnumerator WaitForSecond(float waitTime, Action callBack)
        {
            Debug.Log($"WaitForSecond start, Time: {Time.time}");

            yield return new WaitForSeconds(waitTime);

            callBack?.Invoke();

            Debug.Log($"WaitForSecond finished, Time: {Time.time}");
        }

        private IEnumerator MoveAnimToRagdoll(float clipLength, bool isUp, Action onFinish)
        {
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion rot = Quaternion.LookRotation(dirUp * (isUp ? -1 : 1));
            float y = _mirrorPlayer.AnimTransform.position.y;

            _mirrorPlayer.IsKinematic(true);

            float timer = 0;
            float distance = (_ragdollRoot.position - _mirrorPlayer.AnimTransform.position).magnitude;
            while (distance > _playerSettings.StopDistance && timer < _playerSettings.MoveAnimToRagdollTime)
            {
                Vector3 a = _ragdollRoot.position;
                a.y = y;
                Vector3 b = _mirrorPlayer.AnimTransform.position;
                b.y = y;
                Vector3 pos = Vector3.Lerp(a, b, Time.fixedDeltaTime * Time.fixedDeltaTime);
                
                rot = Quaternion.Lerp(rot, _ragdollRoot.rotation, Time.fixedDeltaTime * Time.fixedDeltaTime);
                
                _mirrorPlayer.SetRigidbodyPositionAndRotation(pos, rot);

                distance = (_ragdollRoot.position - _mirrorPlayer.AnimTransform.position).magnitude;

                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            _mirrorPlayer.IsKinematic(false);

            foreach (var boneController in _boneControllers)
            {
                boneController.IsPositionApplying(true);
                boneController.IsRotationApplying(true);
            }

            if (isUp)
                _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpFaceUp>();
            else
                _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpFaceDown>();

            _coroutineExecuter.Execute(WaitForSecond(clipLength, onFinish.Invoke));

            Debug.Log($"MoveAnimToRagdoll finished, Time: {Time.time}");
        }

        #endregion
    }
}