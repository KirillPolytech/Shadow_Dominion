using UnityEngine;

public class RagdollStabilizer : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private Rigidbody hips;
    [SerializeField] private float speed = 1;
    [SerializeField] private float rotSpeed = 1;
    [SerializeField] private bool isEnableMovement;

    [Header("Debug")] [Space(15)] [Range(0, 1)] [SerializeField]
    private float radius = 0.1f;

    private Transform _centerOfMass;
    private Rigidbody[] _rbs;
    private float x, y;

    private void Awake()
    {
        _rbs = hips.GetComponentsInChildren<Rigidbody>();

        _centerOfMass = new GameObject("CenterOfMass").transform;
    }

    private void Update()
    {
        CalculatingCenterOfMass();
        
        if (!isEnableMovement)
            return;
        
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        
        rb.position = Vector3.Lerp( rb.position,_centerOfMass.position, Time.deltaTime);
        
        Debug.Log($"Centermass: {_centerOfMass} CurrentPos: {rb.position}" +
                  $" " + $"Equals: {_centerOfMass.position == rb.position} ");
    }

    private void FixedUpdate()
    {
        if (!isEnableMovement)
            return;
        
        Vector3 dir = (transform.forward * y + transform.right * x).normalized;
        dir.y = 0;

        rb.linearVelocity = dir * (speed * Time.deltaTime);
        Vector3 lookDir = cameraLook.transform.forward;
        lookDir.y = 0;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(lookDir), rotSpeed * Time.fixedDeltaTime);
    }

    private void CalculatingCenterOfMass()
    {
        _centerOfMass.position = default;

        foreach (var r in _rbs)
        {
            _centerOfMass.position += r.position + r.centerOfMass;
        }

        _centerOfMass.position /= _rbs.Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (_centerOfMass)
            Gizmos.DrawSphere(_centerOfMass.position, radius);
    }
}