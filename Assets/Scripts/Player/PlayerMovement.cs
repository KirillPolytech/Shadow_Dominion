using Mirror;
using UnityEngine;

namespace HellBeavers.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        private float x, y;
        private LegPlacer _legPlacer;
        private Quaternion _cachedRot;
        private Vector3 _dir;
        private MonoInputHandler _inputHandler;
        private PlayerSettings _playerSettings;
        private Rigidbody _charRigidbody;
        private CameraLook _cameraLook;
        private Transform _transform;
        private Vector3 _centerOfMass;

        //[Inject]
        public void Construct(
            PlayerSettings playerSettings,
            Rigidbody characterController,
            CameraLook cameraLook,
            MonoInputHandler inputHandler,
            LegPlacer legPlacer)
        {
            _inputHandler = inputHandler;
            _playerSettings = playerSettings;
            _charRigidbody = characterController;
            _cameraLook = cameraLook;
            _legPlacer = legPlacer;
            _transform = _charRigidbody.transform;

            _inputHandler.OnInputUpdate += HandleInput;
        }

        private void HandleInput(InputData data)
        {
            if (!isLocalPlayer)
                return;
            
            x = data.HorizontalAxisRaw;
            y = data.VerticalAxisRaw;
        }

        public void FixedUpdate()
        {
            _centerOfMass = _transform.position + Vector3.up;
            PlaceLegs();
            Move();
            Rotate();
        }

        private void PlaceLegs()
        {
            if (!_playerSettings.canPlaceLegs)
                return;

            if (x + y == 0 && _transform.rotation == _cachedRot)
                return;

            _legPlacer.Step();

            _cachedRot = _transform.rotation;
        }

        private void Move()
        {
            if (!_playerSettings.canMove)
                return;

            _dir = (_cameraLook.CameraTransform.forward * y +
                    _cameraLook.CameraTransform.right * x).normalized * _playerSettings.speed;
            _dir.y = 0;

            _charRigidbody.AddForce(_dir);

            Debug.DrawRay(_centerOfMass, _dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
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