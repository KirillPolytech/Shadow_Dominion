using Shadow_Dominion.InputSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class CameraLook : NetworkBehaviourInitializer
    {
        public Transform CameraTransform { get; private set; }
        public Vector3 HitPoint { get; private set; }

        public bool CanZooming { get; set; } = true;

        private CinemachineThirdPersonFollow _cinemachineThirdPersonFollow;
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
            CinemachineThirdPersonFollow cinemachineThirdPersonFollow)
        {
            _cameraSettings = camSettings;
            CameraTransform = transform;
            _monoInputHandler = monoInputHandler;
            _cinemachineThirdPersonFollow = cinemachineThirdPersonFollow;

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

            IsInitialized = true;
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
                return;

            Zooming();
        }

        private void HandleInput(InputData inputData)
        {
            CastRay();

            _rightMouseValue = inputData.RightMouseButton ? 1 : 0;
        }

        private void Zooming()
        {
            if (!CanZooming || !_cinemachineThirdPersonFollow)
                return;

            float rightMouseValue = _rightMouseValue == 1 ? -1 : 1;

            _cinemachineThirdPersonFollow.CameraDistance +=
                rightMouseValue * _cameraSettings.ZoomSpeed * Time.fixedDeltaTime;
            _cinemachineThirdPersonFollow.CameraDistance = Mathf.Clamp(_cinemachineThirdPersonFollow.CameraDistance,
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

        private void OnDrawGizmos()
        {
            Debug.DrawLine(_ray.origin, _hit.point, Color.red);
        }
    }
}