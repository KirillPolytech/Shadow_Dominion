using System;
using System.Collections;
using Shadow_Dominion.AnimStateMachine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpFaceUpState : PlayerState
    {
        private readonly RigBuilder _rigBuilder;
        private readonly CameraLook _cameraLook;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly float _clipLength;
        private readonly BoneController[] _boneControllers;
        private readonly Func<float, Action, IEnumerator> _waitingCoroutine;
        private readonly Func<float, bool, IEnumerator> _moveToCoroutine;
        
        public StandUpFaceUpState(
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            CoroutineExecuter coroutineExecuter,
            float clipLength,
            BoneController[] boneControllers,
            Func<float, bool, IEnumerator> moveToCoroutine) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _cameraLook = cameraLook;
            _coroutineExecuter = coroutineExecuter;
            _clipLength = clipLength;
            _boneControllers = boneControllers;
            _moveToCoroutine = moveToCoroutine;
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
            
            _coroutineExecuter.Execute(_moveToCoroutine(_clipLength, true));
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
        }
    }
}