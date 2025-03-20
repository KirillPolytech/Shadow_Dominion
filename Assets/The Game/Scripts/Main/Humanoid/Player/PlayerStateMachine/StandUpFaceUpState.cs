using System;
using System.Collections;
using Shadow_Dominion.AnimStateMachine;
using UnityEditor.Animations;
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
        private readonly Func<float, bool, Action, IEnumerator> _moveToCoroutine;
        private readonly PlayerStateMachine _playerStateMachine;

        private IEnumerator _currentCoroutine;

        public StandUpFaceUpState(
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            CoroutineExecuter coroutineExecuter,
            float clipLength,
            BoneController[] boneControllers,
            Func<float, bool, Action, IEnumerator> moveToCoroutine,
            PlayerStateMachine playerStateMachine) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _cameraLook = cameraLook;
            _coroutineExecuter = coroutineExecuter;
            _clipLength = clipLength;
            _boneControllers = boneControllers;
            _moveToCoroutine = moveToCoroutine;
            _playerStateMachine = playerStateMachine;
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

            _currentCoroutine = _moveToCoroutine(_clipLength, true, () =>
            {
                _currentCoroutine = null;
                _playerStateMachine.SetState<DefaultState>();
            });
            _coroutineExecuter.Execute(_currentCoroutine);
        }

        public override bool CanExit() => _currentCoroutine == null;
    }
}