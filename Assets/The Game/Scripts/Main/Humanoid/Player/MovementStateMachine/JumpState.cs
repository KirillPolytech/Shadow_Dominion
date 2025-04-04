using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Shadow_Dominion.StateMachine
{
    public class JumpState : MovementState
    {
        private readonly CameraLook _cameraLook;
        private readonly Rigidbody _charRigidbody;
        private readonly PlayerSettings _playerSettings;
        private readonly Transform _transform;
        private readonly PlayerAnimation _playerAnimation;
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerMovement _playerMovement;

        private float _delay = 0.5f, _timer;
        private bool _finishJump;

        public JumpState(
            CameraLook cameraLook,
            Rigidbody charRigidbody,
            PlayerSettings playerSettings,
            PlayerAnimation playerAnimation,
            PlayerStateMachine playerStateMachine,
            PlayerMovement playerMovement)
        {
            _cameraLook = cameraLook;
            _charRigidbody = charRigidbody;
            _playerSettings = playerSettings;
            _transform = _charRigidbody.transform;
            _playerAnimation = playerAnimation;
            _playerStateMachine = playerStateMachine;
            _playerMovement = playerMovement;
        }

        public override void Enter()
        {
            Jump();
            _finishJump = false;
            _timer = 0;
        }

        public override void Update(InputData inputData)
        {
            Move(inputData);
            _playerMovement.Rotate(inputData);

            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, _delay + 1);

            if (_timer < _delay)
                return;

            _finishJump = true;
        }
        
        private void Jump()
        {
            Vector3 dir = new Vector3(0, _playerSettings.JumpForce, 0);
            
            _charRigidbody.AddForce(dir);

            // Debug.Log($"x:{x} y:{y} dir: {dir} isRunInt: {isRunInt}");

            Debug.DrawRay(_transform.position + Vector3.up, dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }

        private void Move(InputData inputData)
        {
            Vector3 camForward = new Vector3(_cameraLook.CameraTransform.forward.x, 0,
                _cameraLook.CameraTransform.forward.z);
            Vector3 camRight = new Vector3(_cameraLook.CameraTransform.right.x, 0,
                _cameraLook.CameraTransform.right.z);

            Vector3 dir = (camForward * inputData.VerticalAxisRaw + camRight * inputData.HorizontalAxisRaw).normalized;

            dir *= _playerSettings.WalkSpeed + _playerSettings.RunSpeed;

            _charRigidbody.AddForce(dir);
            Vector3 clamped = Vector3.ClampMagnitude(_charRigidbody.linearVelocity, _playerSettings.MaxWalkSpeed);
            _charRigidbody.linearVelocity =
                new Vector3(clamped.x, _charRigidbody.linearVelocity.y, clamped.z);
        }

        public override bool CanExit() => _finishJump && _playerMovement.OnGround();

        public override void Exit()
        {
        }
    }
}