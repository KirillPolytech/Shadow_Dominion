using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class RagdollState : IState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimation _playerAnimation;
        private readonly RigBuilder _rigBuilder;
        private readonly BoneController[] _boneControllers;
        private readonly Vector3 _forceDirection;

        public RagdollState(
            PlayerMovement playerMovement,
            PlayerAnimation playerAnimation,
            RigBuilder rigBuilder,
            BoneController[] boneControllers)
        {
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;
            _rigBuilder = rigBuilder;
            _boneControllers = boneControllers;
            _forceDirection = Vector3.zero;
        }

        public override void Enter()
        {
            _rigBuilder.enabled = false;
            _playerMovement.CanMove = false;
            _playerAnimation.CanAnimate = false;
            
            for (int i = 0; i < _boneControllers.Length; i++)
            {
                //_boneControllers[i].IsPositionApplying(false);
                //_boneControllers[i].IsRotationApplying(false);
                _boneControllers[i].AddForce(_forceDirection);
                //boneController[i].UpdateSpring(isEnabled);
            }

            _playerAnimation.AnimationStateMachine.SetState<LayingState>();
            
            //_playerAnimation.Animator
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _playerMovement.CanMove = true;
            _playerAnimation.CanAnimate = true;
            
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