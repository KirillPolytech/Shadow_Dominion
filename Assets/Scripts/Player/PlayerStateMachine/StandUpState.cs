using Shadow_Dominion.Main;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpState : PlayerState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly RigBuilder _rigBuilder;
        private readonly Main.Player _player;
        private readonly Transform _ragdollRoot;
        
        public StandUpState(
            Main.Player player,
            Transform ragdollRoot,
            PlayerMovement playerMovement, 
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _rigBuilder = rigBuilder;
            _player = player;
            _ragdollRoot = ragdollRoot;
        }
        
        public override void Enter()
        {
            _playerMovement.CanMove = false;
            _rigBuilder.enabled = false;
            
            _player.SetPositionAndRotation(_ragdollRoot.position, _ragdollRoot.up);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpState>();

            _playerAnimation.StartStandUp();
        }

        public override void Exit()
        {
            _playerMovement.CanMove = true;
            _rigBuilder.enabled = true;
        }
    }
}