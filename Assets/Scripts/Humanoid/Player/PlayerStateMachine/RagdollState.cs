using Shadow_Dominion.AnimStateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class RagdollState : PlayerState
    {
        private readonly RigBuilder _rigBuilder;
        private readonly BoneController[] _boneControllers;
        private readonly CameraLook _cameraLook;
        private readonly Vector3 _forceDirection;

        public RagdollState(
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            RigBuilder rigBuilder,
            BoneController[] boneControllers) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _boneControllers = boneControllers;
            _cameraLook = cameraLook;
            _forceDirection = Vector3.zero;
        }

        public override void Enter()
        {
            _rigBuilder.enabled = false;
            _playerAnimation.CanAnimate = false;
            _cameraLook.CanZooming = false;
            
            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(false);
                _boneControllers[i].IsRotationApplying(false);
                _boneControllers[i].AddForce(_forceDirection);
                //boneController[i].UpdateSpring(isEnabled);
            }

            _playerAnimation.AnimationStateMachine.SetState<AnimationLayingState>();
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _playerAnimation.CanAnimate = true;
            _cameraLook.CanZooming = true;
            
            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(true);
                _boneControllers[i].IsRotationApplying(true);
                _boneControllers[i].AddForce(Vector3.zero);
                //boneController[i].UpdateSpring(isEnabled);
            }
        }
    }
}