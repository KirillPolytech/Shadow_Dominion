using DG.Tweening;
using Mirror;
using Unity.Cinemachine;
using UnityEngine;

public class CameraLook : NetworkBehaviour
{
    [SerializeField] private CinemachineFreeLookModifier freeLookModifier;
    [SerializeField] private Transform target;
    [SerializeField] private Transform aimPos;
    [SerializeField] private Transform defaultPos;
    
    public Transform CameraTransform { get; private set; }
    public Vector3 HitPoint { get; private set; }

    private RaycastHit _hit;
    private Ray _ray;
    private CameraSettings _cameraSettings;
    private Camera _camera;
    
    public void Construct(CameraSettings camSettings)
    {
        _cameraSettings = camSettings;
        CameraTransform = transform;

        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (!isLocalPlayer)
            _camera.gameObject.SetActive(false);
    }

    private void Update()
    {
        CastRay();
        
        return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            target.DOMove(aimPos.position,1);
        }
        else
        {
            target.DOMove(defaultPos.position,1);
        }
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
