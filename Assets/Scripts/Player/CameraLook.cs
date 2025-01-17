using Mirror;
using Unity.Cinemachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class CameraLook : NetworkBehaviour
    {
        public Transform CameraTransform { get; private set; }
        public Vector3 HitPoint { get; private set; }

        private CinemachineThirdPersonFollow _cinemachineThirdPersonFollow;
        private MonoInputHandler _monoInputHandler;
        private CameraSettings _cameraSettings;
        private Camera _camera;
        
        private RaycastHit _hit;
        private Ray _ray;

        public void Construct(
            CameraSettings camSettings,
            MonoInputHandler monoInputHandler, 
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

        private void HandleInput(InputData inputData)
        {
            CastRay();

            _cinemachineThirdPersonFollow.CameraDistance = inputData.RightMouseButton ? 0 : _cameraSettings.zoom;
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