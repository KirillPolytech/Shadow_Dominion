using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Player;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class PlayerMovement
    {
        private PlayerSettings _playerSettings;
        private CameraLook _cameraLook;
        private Rigidbody _charRigidbody;
        private PlayerAnimation _playerAnimation;

        private Transform _transform;
        private Quaternion _cachedRot;

        public void Construct(
            PlayerSettings playerSettings,
            Rigidbody characterController,
            CameraLook cameraLook,
            PlayerAnimation playerAnimation)
        {
            _playerSettings = playerSettings;
            _charRigidbody = characterController;
            _cameraLook = cameraLook;
            _transform = _charRigidbody.transform;
            _playerAnimation = playerAnimation;
        }

        public void HandleInput(InputData data)
        {
            Move(data.LeftShift, data.HorizontalAxisRaw, data.VerticalAxisRaw);
            Rotate();
            HandleAnim(data);
        }

        // todo: refactor
        private void HandleAnim(InputData data)
        {
            float magnitude = new Vector3 (_charRigidbody.linearVelocity.x, 0 , _charRigidbody.linearVelocity.z).magnitude;
            Vector2 dir = new Vector2(data.HorizontalAxisRaw, data.VerticalAxisRaw);
            
            float speedDen = data.LeftShift ? _playerSettings.MaxRunSpeed : _playerSettings.MaxWalkSpeed;
            float x = dir.x * magnitude / speedDen;
            float y = dir.y * magnitude / speedDen;

            x = Mathf.Clamp(x, -1, 1);
            y = Mathf.Clamp(y, -1, 1);

            _playerAnimation.AnimationStateMachine.SetXY(x, y);
            
            // Debug.Log($"magnitude: {magnitude} " + $"x = {dir.x * magnitude} / {_playerSettings.RunSpeed} " + $"y = {dir.y * magnitude} / {_playerSettings.RunSpeed}");
        }

        private void Move(bool isRun, float x, float y)
        {
            if (!_playerSettings.CanMove)
                return;

            int isRunInt = isRun ? 1 : 0;

            Vector3 camForward = new Vector3(_cameraLook.CameraTransform.forward.x, 0,
                _cameraLook.CameraTransform.forward.z);
            Vector3 camRight = new Vector3(_cameraLook.CameraTransform.right.x, 0,
                _cameraLook.CameraTransform.right.z);
            
            Vector3 dir = (camForward * y + camRight * x).normalized;
            
            dir *= _playerSettings.WalkSpeed * (1 - isRunInt) +
                   _playerSettings.RunSpeed * isRunInt; //* (dir.y < 0 ? 1 : 0.5f);
            dir.y = Physics.gravity.y;

            _charRigidbody.AddForce(dir);
            _charRigidbody.linearVelocity = Vector3.ClampMagnitude(_charRigidbody.linearVelocity, 
                isRun ? _playerSettings.MaxRunSpeed : _playerSettings.MaxWalkSpeed);
            
            // Debug.Log($"x:{x} y:{y} dir: {dir} isRunInt: {isRunInt}");

            Debug.DrawRay(_transform.position + Vector3.up, dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }

        // todo: refactor
        private void Rotate()
        {
            if (!_playerSettings.CanRotate)
                return;

            Vector3 transformForward =
                new Vector3(_cameraLook.CameraTransform.forward.x, 0, _cameraLook.CameraTransform.forward.z);

            //transformForward.y = dir == default ? 0 : Mathf.Sign(dir.y) * _playerSettings.tilt;

            Quaternion rot = Quaternion.Lerp(_charRigidbody.rotation,
                Quaternion.LookRotation(transformForward),
                _playerSettings.RotSpeed * Time.fixedDeltaTime);

            _charRigidbody.MoveRotation(rot);
        }
    }
}