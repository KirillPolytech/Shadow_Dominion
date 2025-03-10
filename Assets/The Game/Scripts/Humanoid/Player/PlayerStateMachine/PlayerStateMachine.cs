using System;
using System.Collections;
using System.Linq;
using Mirror;
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
        private readonly Main.Player _player;
        private readonly Transform _ragdollRoot;
        private readonly PlayerAnimation _playerAnimation;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly BoneController[] _boneControllerses;
        private readonly IInputHandler _inputHandler;
        
        public PlayerStateMachine(
            Main.Player player,
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
            PositionMessage[] positionMessage)
        {
            _player = player;
            _ragdollRoot = ragdollRoot;
            _playerAnimation = playerAnimation;
            _coroutineExecuter = coroutineExecuter;
            _boneControllerses = boneControllers;
            _inputHandler = inputHandler;

            StandUpFaceUpState standUpFaceUpState =
                new StandUpFaceUpState(rootRig, playerAnimation,
                    cameraLook, coroutineExecuter, standUpFaceUp.length, boneControllers, MoveTo);
            StandUpFaceDownState standUpFaceDownState =
                new StandUpFaceDownState( rootRig, playerAnimation,
                    cameraLook, coroutineExecuter, standUpFaceDown.length, boneControllers, MoveTo);
            RagdollState ragdollState =
                new RagdollState(playerAnimation, cameraLook, rootRig, boneControllers, inputHandler, ragdollRoot,
                    this);
            DeathState deathState = new DeathState(playerAnimation, boneControllers, cameraLook);
            DefaultState defaultState = new DefaultState(playerAnimation, playerMovement, inputHandler, windowsController);
            InActiveState inActiveState = new InActiveState(player, positionMessage);
            PauseMenuState pauseMenuState = new PauseMenuState(windowsController);

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
                SetState<PauseMenuState>();
                return;
            }

            SetState<DefaultState>();
        }

        private IEnumerator WaitForSecond(float waitTime, Action callBack)
        {
            yield return new WaitForSeconds(waitTime);

            callBack?.Invoke();
        }

        private IEnumerator MoveTo(float clipLength, bool isUp)
        {
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion q = Quaternion.LookRotation(dirUp * (isUp ? -1 : 1) );
            float y = _player.PlayersTrasform.position.y;

            _player.IsKinematic(true);

            float distance = (_ragdollRoot.position - _player.PlayersTrasform.position).magnitude;
            while (distance > 0.25f)
            {
                Vector3 a = _ragdollRoot.position;
                a.y = y;
                Vector3 b = _player.PlayersTrasform.position;
                b.y = y;
                Vector3 pos = Vector3.Lerp(a, b, Time.fixedDeltaTime * Time.fixedDeltaTime);
                _player.SetPositionAndRotation(pos, q);

                distance = (_ragdollRoot.position - _player.PlayersTrasform.position).magnitude;
                yield return new WaitForFixedUpdate();
            }

            _player.IsKinematic(false);

            foreach (var boneController in _boneControllerses)
            {
                boneController.IsPositionApplying(true);
                boneController.IsRotationApplying(true);
            }

            _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpFaceUp>();

            _coroutineExecuter.Execute(WaitForSecond(clipLength, SetState<DefaultState>));
        }

        public override void SetState<T>()
        {
            IState state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            NetworkClient.Send(new PlayerStateMessage(CurrentState.GetType().ToString()));

            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }

        public void SetState(string stateName)
        {
            IState state = _states.First(x => x.GetType().ToString() == stateName);

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
            
            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
    }
}