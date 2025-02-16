using System;
using System.Collections;
using Shadow_Dominion.AnimStateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpFaceUpState : PlayerState
    {
        private readonly RigBuilder _rigBuilder;
        private readonly Main.Player _player;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly float _clipLength;
        private readonly BoneController[] _boneControllers;
        private readonly Func<float, Action, IEnumerator> _waitingCoroutine;
        
        public StandUpFaceUpState(
            Main.Player player,
            Transform ragdollRoot,
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            CoroutineExecuter coroutineExecuter,
            PlayerStateMachine playerStateMachine,
            float clipLength,
            BoneController[] boneControllers,
            Func<float, Action, IEnumerator> waitingCoroutine) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _player = player;
            _ragdollRoot = ragdollRoot;
            _cameraLook = cameraLook;
            _coroutineExecuter = coroutineExecuter;
            _playerStateMachine = playerStateMachine;
            _clipLength = clipLength;
            _boneControllers = boneControllers;
            _waitingCoroutine = waitingCoroutine;
        }
        
        public override void Enter()
        {
            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationLay>();
            
            foreach (var boneController in _boneControllers)
            {
                boneController.IsPositionApplying(false);
                boneController.IsRotationApplying(false);
            }
            
            _coroutineExecuter.Execute(MoveTo());
        }
        
        private IEnumerator MoveTo()
        {
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion q = Quaternion.LookRotation(-dirUp);
            float y = _player.transform.position.y;
            
            _player.IsKinematic( true);
            
            float distance = (_ragdollRoot.position - _player.transform.position).magnitude;
            while (distance > 0.25f)
            {
                Vector3 a = _ragdollRoot.position;
                a.y = y;
                Vector3 b = _player.transform.position;
                b.y = y;
                Vector3 pos = Vector3.Lerp(a, b, Time.fixedDeltaTime * Time.fixedDeltaTime);
                _player.SetPositionAndRotation(pos, q);

                distance = (_ragdollRoot.position - _player.transform.position).magnitude;
                yield return new WaitForFixedUpdate();
            }
            
            _player.IsKinematic( false);
            
            foreach (var boneController in _boneControllers)
            {
                boneController.IsPositionApplying(true);
                boneController.IsRotationApplying(true);
            }
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpFaceUp>();

            _coroutineExecuter.Execute(_waitingCoroutine(_clipLength, _playerStateMachine.SetState<DefaultState>));
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
        }
    }
}