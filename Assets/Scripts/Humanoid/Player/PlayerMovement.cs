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
        private float _x, _y;

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
            int leftShift = data.LeftShift ? 1 : 0;

            float temp = 0.01f;

            float newXvalue = _x + data.HorizontalAxisRaw * temp;
            newXvalue *= data.HorizontalAxisRaw == 0 ? 0 : 1;
            _x = Mathf.Clamp(newXvalue, -1f,1f);
            
            float newYvalue = _y + data.VerticalAxisRaw * temp;
            newYvalue *= data.VerticalAxisRaw == 0 ? 0 : 1;

            //float runY = Mathf.Clamp(data.VerticalAxisRaw * leftShift * temp, -0.5f, 0.5f);

            _y = Mathf.Clamp(newYvalue, -0.5f, 0.5f); //+ runY;
            
            _playerAnimation.AnimationStateMachine.SetXY(_x, _y); //data.VerticalAxisRaw  / 2 + 0.5f * leftShift);
        }

        private void Move(bool isRun, float x, float y)
        {
            if (!_playerSettings.canMove)
                return;

            int isRunInt = isRun ? 1 : 0;

            Vector3 dir = (_cameraLook.CameraTransform.forward * y +
                            _cameraLook.CameraTransform.right * x).normalized;
            dir *= _playerSettings.walkSpeed * (1 - isRunInt) + _playerSettings.runSpeed * isRunInt;
            dir.y = Physics.gravity.y;

            _charRigidbody.linearVelocity = dir;

            Debug.DrawRay(_transform.position + Vector3.up, dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }

        // todo: refactor
        private void Rotate()
        {
            if (!_playerSettings.canRotate)
                return;

            Vector3 transformForward =
                new Vector3(_cameraLook.CameraTransform.forward.x, 0, _cameraLook.CameraTransform.forward.z);

            //transformForward.y = dir == default ? 0 : Mathf.Sign(dir.y) * _playerSettings.tilt;

            Quaternion rot = Quaternion.Lerp(_charRigidbody.rotation,
                Quaternion.LookRotation(transformForward),
                _playerSettings.rotSpeed * Time.fixedDeltaTime);

            _charRigidbody.MoveRotation(rot);
        }
    }
}