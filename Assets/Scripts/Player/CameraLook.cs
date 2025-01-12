using DG.Tweening;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class CameraLook : NetworkBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform aimPos;
        [SerializeField] private Transform defaultPos;

        [SerializeField] private float transitDuration = 0.5f;

        public Transform CameraTransform { get; private set; }
        public Vector3 HitPoint { get; private set; }

        private RaycastHit _hit;
        private Ray _ray;
        private CameraSettings _cameraSettings;
        private Camera _camera;
        private MonoInputHandler _monoInputHandler;

        public void Construct(CameraSettings camSettings, MonoInputHandler monoInputHandler)
        {
            _cameraSettings = camSettings;
            CameraTransform = transform;
            _monoInputHandler = monoInputHandler;

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

            target.DOMove(inputData.RightMouseButton ? aimPos.position : defaultPos.position, transitDuration);
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