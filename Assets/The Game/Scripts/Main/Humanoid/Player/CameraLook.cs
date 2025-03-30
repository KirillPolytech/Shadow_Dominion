using Mirror;
using Shadow_Dominion.InputSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class CameraLook : NetworkBehaviour
    {
        private const int HalfCircleAngle = 180;
        
        public Transform CameraTransform { get; private set; }
        public Vector3 HitPoint { get; private set; }

        public bool CanZooming { get; set; } = true;

        private CinemachineOrbitalFollow _cinemachinePositionType;
        private CinemachineRotationComposer _cinemachineRotationType;
        private CinemachineInputAxisController _cinemachineInputAxisController;
        private IInputHandler _monoInputHandler;
        private CameraSettings _cameraSettings;
        private Camera _camera;

        private RaycastHit _hit;
        private Ray _ray;
        private int _rightMouseValue;
        private float _currentScrollDistance;
        private float _aimDistance;

        public void Construct(
            CameraSettings camSettings,
            IInputHandler monoInputHandler,
            CinemachineComponentBase cinemachinePositionType,
            CinemachineComponentBase cinemachineRotationType,
            CinemachineInputAxisController cinemachineInputAxisController)
        {
            _cameraSettings = camSettings;
            CameraTransform = transform;
            _monoInputHandler = monoInputHandler;
            _cinemachinePositionType = cinemachinePositionType as CinemachineOrbitalFollow;
            _cinemachineRotationType = cinemachineRotationType as CinemachineRotationComposer;
            _cinemachineInputAxisController = cinemachineInputAxisController;
            
            _camera = GetComponent<Camera>();

            _monoInputHandler.OnInputUpdate += HandleInput;
        }

        private void OnDestroy()
        {
            if (_monoInputHandler == null)
                return;

            _monoInputHandler.OnInputUpdate -= HandleInput;
        }

        private void Start()
        {
            if (!isLocalPlayer)
                _camera.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            Zooming();
        }

        private void HandleInput(InputData inputData)
        {
            CastRay();

            _rightMouseValue = inputData.RightMouseButton ? 1 : 0;
        }

        public void CanRotate(bool canRotate)=>
            _cinemachineInputAxisController.enabled = canRotate;
        
        private void Zooming()
        {
            if (!CanZooming || !_cinemachinePositionType)
                return;

            float rightMouseValue = _rightMouseValue == 1 ? -1 : 1;

            _cinemachinePositionType.Radius +=
                rightMouseValue * _cameraSettings.ZoomSpeed * Time.fixedDeltaTime;
            _cinemachinePositionType.Radius = Mathf.Clamp(_cinemachinePositionType.Radius,
                _cameraSettings.cameraMinDistance, _cameraSettings.cameraMaxDistance);
        }

        private void CastRay()
        {
            _ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(_ray, out _hit, _cameraSettings.rayCastDistance, ~_cameraSettings.layerMask))
            {
                HitPoint = _hit.point;
                return;
            }

            _hit.point = _ray.GetPoint(_cameraSettings.rayCastDistance);

            HitPoint = _hit.point;
        }

        public void SetRotation(Quaternion rot)
        {
            _cinemachinePositionType.HorizontalAxis.Value = Quaternion.Angle(transform.rotation, rot) - HalfCircleAngle;
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(_ray.origin, _hit.point, Color.red);
        }
    }
}