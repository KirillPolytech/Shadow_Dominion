using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class RagdollState : PlayerState
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly RigBuilder _rigBuilder;
        private readonly IInputHandler _inputHandler;
        private readonly BoneController[] _boneControllers;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        private readonly Vector3 _forceDirection;

        public RagdollState(
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            RigBuilder rigBuilder,
            BoneController[] boneControllers,
            IInputHandler inputHandler,
            Transform ragdollRoot,
            PlayerStateMachine playerStateMachine) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _boneControllers = boneControllers;
            _cameraLook = cameraLook;
            _forceDirection = Vector3.zero;
            _inputHandler = inputHandler;
            _ragdollRoot = ragdollRoot;
            _playerStateMachine = playerStateMachine;
        }

        private void HandleInput(InputData inputData)
        {
            Vector3 upDirection = _ragdollRoot.up;
            float angle = Vector3.Angle(upDirection, Vector3.up);

            if (angle < 90f)
                return;

            //if (!inputData.F_Down)
                //return;

            if (Vector3.Dot(_ragdollRoot.forward, Vector3.up) > 0)
                _playerStateMachine.SetState<StandUpFaceUpState>();
            else
                _playerStateMachine.SetState<StandUpFaceDownState>();
        }

        public override void Enter()
        {
            _inputHandler.OnInputUpdate += HandleInput;

            _playerAnimation.AnimationStateMachine.SetState<AnimationLay>();

            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;

            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(false);
                _boneControllers[i].IsRotationApplying(false);
                _boneControllers[i].AddForce(_forceDirection);
            }
        }

        public override void Exit()
        {
            _inputHandler.OnInputUpdate -= HandleInput;
        }

        public override bool CanExit() => true;
    }
}