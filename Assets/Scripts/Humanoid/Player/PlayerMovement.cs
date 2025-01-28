using Mirror;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class PlayerMovement : NetworkBehaviour
    {
        public bool CanMove { get; set; } = true;
        
        private MonoInputHandler _inputHandler;
        private PlayerSettings _playerSettings;
        private CameraLook _cameraLook;
        private Rigidbody _charRigidbody;

        private Transform _transform;
        private Quaternion _cachedRot;
        private Vector3 _dir, _centerOfMass;
        private float x, y;
        private bool _isRun;

        public void Construct(
            PlayerSettings playerSettings,
            Rigidbody characterController,
            CameraLook cameraLook,
            MonoInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _playerSettings = playerSettings;
            _charRigidbody = characterController;
            _cameraLook = cameraLook;
            _transform = _charRigidbody.transform;

            _inputHandler.OnInputUpdate += HandleInput;
        }

        private void HandleInput(InputData data)
        {
            if (!isLocalPlayer)
                return;
            
            x = data.HorizontalAxisRaw;
            y = data.VerticalAxisRaw;
            _isRun = data.LeftShift;
        }

        public void FixedUpdate()
        {
            if (!CanMove)
                return;
            
            _centerOfMass = _transform.position + Vector3.up;
            Move();
            Run();
            Rotate();
        }

        private void Move()
        {
            if (!_playerSettings.canMove || _isRun)
                return;

            _dir = (_cameraLook.CameraTransform.forward * y +
                    _cameraLook.CameraTransform.right * x).normalized * _playerSettings.walkSpeed;
            _dir.y = 0;

            _charRigidbody.AddForce(_dir);

            Debug.DrawRay(_centerOfMass, _dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }
        
        private void Run()
        {
            if (!_playerSettings.canMove || !_isRun)
                return;

            Vector3 forward = new Vector3(_cameraLook.CameraTransform.forward.x, 0,
                _cameraLook.CameraTransform.forward.z);
            
            Vector3 right = new Vector3(_cameraLook.CameraTransform.right.x, 0,
                _cameraLook.CameraTransform.right.z);
            
            _dir = (forward * y + right * x).normalized * _playerSettings.runSpeed;

            _charRigidbody.AddForce(_dir);
        }

        private void Rotate()
        {
            if (!_playerSettings.canRotate)
                return;

            Vector3 transformForward = new Vector3(_cameraLook.CameraTransform.forward.x, 0, _cameraLook.CameraTransform.forward.z);

            transformForward.y = _dir == default ? 0 : Mathf.Sign(_dir.y) * _playerSettings.tilt;

            Quaternion rot = Quaternion.Lerp(_charRigidbody.rotation,
                Quaternion.LookRotation(transformForward),
                _playerSettings.rotSpeed * Time.fixedDeltaTime);
            
            _charRigidbody.MoveRotation(rot);
        }

        public void OnDestroy()
        {
            _inputHandler.OnInputUpdate += HandleInput;
        }
    }
}