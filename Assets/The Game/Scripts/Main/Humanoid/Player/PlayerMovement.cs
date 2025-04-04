using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class PlayerMovement
    {
        public MovementStateMachine MovementMachine { get; private set; }

        private PlayerSettings _playerSettings;
        private CameraLook _cameraLook;
        private Rigidbody _charRigidbody;
        private PlayerAnimation _playerAnimation;
        private PlayerStateMachine _playerStateMachine;

        public void Construct(
            PlayerSettings playerSettings,
            Rigidbody characterController,
            CameraLook cameraLook,
            PlayerAnimation playerAnimation,
            PlayerStateMachine playerStateMachine)
        {
            _playerSettings = playerSettings;
            _charRigidbody = characterController;
            _cameraLook = cameraLook;
            _playerAnimation = playerAnimation;
            _playerStateMachine = playerStateMachine;

            MovementMachine = new MovementStateMachine(
                _cameraLook, 
                _charRigidbody, 
                _playerSettings, 
                _playerAnimation, 
                _playerStateMachine,
                this);
        }

        public void HandleInput(InputData data)
        {
            if (data.SPACE_DOWN)
                MovementMachine.SetState<JumpState>();
            if (data.LeftShift)
                MovementMachine.SetState<RunState>();
            else
                MovementMachine.SetState<WalkState>();
            
            MovementMachine.CurrentState?.Update(data);
        }
        
        public bool OnGround()
        {
            Ray ray = new Ray(_charRigidbody.position + Vector3.up, Vector3.down);

            Debug.DrawRay(ray.origin, Vector3.down, Color.red);

            LayerMask mask = ~LayerMask.GetMask(_playerSettings.FallRayMask);
            return Physics.Raycast(ray, _playerSettings.GroundCheckDistance, mask);
        }
    }
}