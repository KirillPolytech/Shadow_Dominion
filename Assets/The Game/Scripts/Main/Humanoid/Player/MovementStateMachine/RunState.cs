using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.StateMachine
{
    public class RunState : MovementState
    {
        private readonly CameraLook _cameraLook;
        private readonly Rigidbody _charRigidbody;
        private readonly PlayerSettings _playerSettings;
        private readonly Transform _transform;
        private readonly PlayerAnimation _playerAnimation;
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerMovement _playerMovement;

        public RunState(
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
        }

        public override void Update(InputData inputData)
        {
            HandleAnim(inputData);
            Walk(inputData.HorizontalAxisRaw, inputData.VerticalAxisRaw);
            _playerMovement.Rotate(inputData);
            if (!_playerMovement.OnGround())
            {
                _playerStateMachine.SetState<RagdollState>();
            }
        }

        public override bool CanExit() => true;

        private void Walk(float x, float y)
        {
            if (_charRigidbody.isKinematic)
                return;

            Vector3 camForward = new Vector3(_cameraLook.CameraTransform.forward.x, 0,
                _cameraLook.CameraTransform.forward.z);
            Vector3 camRight = new Vector3(_cameraLook.CameraTransform.right.x, 0,
                _cameraLook.CameraTransform.right.z);

            Vector3 dir = (camForward * y + camRight * x).normalized;

            dir *= _playerSettings.RunSpeed;
            dir.y = Physics.gravity.y;

            _charRigidbody.AddForce(dir);
            _charRigidbody.linearVelocity =
                Vector3.ClampMagnitude(_charRigidbody.linearVelocity, _playerSettings.MaxRunSpeed);

            // Debug.Log($"x:{x} y:{y} dir: {dir} isRunInt: {isRunInt}");

            Debug.DrawRay(_transform.position + Vector3.up, dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }

        private void HandleAnim(InputData data)
        {
            float magnitude = new Vector3(_charRigidbody.linearVelocity.x, 0, _charRigidbody.linearVelocity.z)
                .magnitude;
            Vector2 dir = new Vector2(data.HorizontalAxisRaw, data.VerticalAxisRaw);

            float speedDen = _playerSettings.MaxWalkSpeed;
            float x = dir.x * magnitude / speedDen;
            float y = dir.y * magnitude / speedDen;

            x = Mathf.Clamp(x, -2, 2);
            y = Mathf.Clamp(y, -2, 2);

            _playerAnimation.AnimationStateMachine.SetXY(x, y);
            _playerAnimation.AnimationStateMachine.IsCrouching(data.LeftCTRL);

            // Debug.Log($"magnitude: {magnitude} " + $"x = {dir.x * magnitude} / {_playerSettings.RunSpeed} " + $"y = {dir.y * magnitude} / {_playerSettings.RunSpeed}");
        }

        public override void Exit()
        {
        }
    }
}