using Mirror;
using Shadow_Dominion.InputSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class CameraLook : NetworkBehaviour
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
        private float _mouseWheelValue;
        private float _currentScrollDistance;

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
        }

        private void OnEnable()
        {
            _monoInputHandler.OnInputUpdate += HandleInput;
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
            _mouseWheelValue = inputData.MouseWheelScroll;
        }

        private void Zooming()
        {
            if (CanZooming == false)
                return;
            
            float rightMouseValue = _rightMouseValue == 1 ? -1 : 1;
            float cameraDistance = Mathf.Clamp(_cinemachineThirdPersonFollow.CameraDistance +
                                               rightMouseValue * _cameraSettings.zoomDuration * Time.fixedDeltaTime,
                _cameraSettings.zoomInDistance, _cameraSettings.zoom);

            rightMouseValue = _rightMouseValue == 1 ? 0 : 1;
            _currentScrollDistance = Mathf.Clamp( _currentScrollDistance - _mouseWheelValue * Time.fixedDeltaTime, 
                0, _cameraSettings.maxZoomDistance);
            float scrollDistance = _currentScrollDistance * rightMouseValue;
            
            _cinemachineThirdPersonFollow.CameraDistance = cameraDistance + scrollDistance;
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

        private void OnDisable()
        {
            _monoInputHandler.OnInputUpdate -= HandleInput;
        }
    }
}