using Shadow_Dominion.Main;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpFaceUpState : PlayerState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly RigBuilder _rigBuilder;
        private readonly Main.Player _player;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        
        public StandUpFaceUpState(
            Main.Player player,
            Transform ragdollRoot,
            PlayerMovement playerMovement, 
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _rigBuilder = rigBuilder;
            _player = player;
            _ragdollRoot = ragdollRoot;
            _cameraLook = cameraLook;
        }
        
        public override void Enter()
        {
            _playerMovement.CanMove = false;
            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;
            
            Vector3 pos = _ragdollRoot.position;
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion q = Quaternion.LookRotation(-dirUp);
            _player.SetPositionAndRotation(pos, q);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationStandUpFaceUpState>();

            _playerAnimation.StartStandUp();
        }

        public override void Exit()
        {
            _playerMovement.CanMove = true;
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
        }
    }
}