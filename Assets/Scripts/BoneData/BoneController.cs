using UnityEngine;

public class BoneController : MonoBehaviour
{
    public Vector3 CurrentPosition => _rigidbody.position;
    public Quaternion CurrentRotation => _rigidbody.rotation;
    public bool CurrentPosState { get; private set; } = true;
    public bool CurrentRotState { get; private set; } = true;
    public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

    [SerializeField] private ConfigurableJoint _configurableJoint;
    [SerializeField] private Rigidbody _rigidbody;

    private Quaternion _cachedStartRot;

    public void Initialize()
    {
        _configurableJoint = GetComponent<ConfigurableJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _cachedStartRot = transform.localRotation;
    }

    public void SetJointLimits(
        float angularYLimit,
        float angularZLimit,
        float highAngularXLimit,
        float lowAngularXLimit,
        float linearLimit,
        float linearLimitSpring,
        float linearLimitSpringDamper)
    {
        if (!_configurableJoint)
            return;

        _configurableJoint.angularYLimit = new SoftJointLimit { limit = angularYLimit };
        _configurableJoint.angularZLimit = new SoftJointLimit { limit = angularZLimit };
        _configurableJoint.highAngularXLimit = new SoftJointLimit { limit = highAngularXLimit };
        _configurableJoint.lowAngularXLimit = new SoftJointLimit { limit = lowAngularXLimit };
        _configurableJoint.linearLimit = new SoftJointLimit { limit = linearLimit };
        _configurableJoint.linearLimitSpring = new SoftJointLimitSpring
            { spring = linearLimitSpring, damper = linearLimitSpringDamper };
    }

    public void SetDrive(
        float posMaximumForce,
        float positionSpring,
        float positionDamper,
        bool usePosAcceleration,
        float angularMaxForce,
        float angularPositionSpring,
        float angularPositionDamper,
        bool useRotAcceleration)
    {
        if (!_configurableJoint)
            return;

        JointDrive drive = new JointDrive
        {
            maximumForce = posMaximumForce,
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            useAcceleration = usePosAcceleration
        };

        JointDrive angularXDrive = new JointDrive
        {
            positionSpring = angularPositionSpring,
            maximumForce = angularMaxForce,
            positionDamper = angularPositionDamper,
            useAcceleration = useRotAcceleration
        };

        _configurableJoint.angularXDrive = angularXDrive;
        _configurableJoint.angularYZDrive = angularXDrive;

        _configurableJoint.xDrive = drive;
        _configurableJoint.yDrive = drive;
        _configurableJoint.zDrive = drive;
    }

    public void SetPositionMotionState(ConfigurableJointMotion configurableJointMotion)
    {
        if (!_configurableJoint)
            return;

        _configurableJoint.xMotion = configurableJointMotion;
        _configurableJoint.yMotion = configurableJointMotion;
        _configurableJoint.zMotion = configurableJointMotion;
    }

    public void SetRotationMotionState(ConfigurableJointMotion configurableJointMotion)
    {
        if (!_configurableJoint)
            return;

        _configurableJoint.angularXMotion = configurableJointMotion;
        _configurableJoint.angularYMotion = configurableJointMotion;
        _configurableJoint.angularZMotion = configurableJointMotion;
    }

    public void SetTargets(
        Vector3 targetPosition,
        Quaternion targetRotation,
        Vector3 targetVelocity)
    {
        if (!_configurableJoint)
            return;

        _configurableJoint.targetPosition = targetPosition;
        _configurableJoint.targetRotation = targetRotation;
        _configurableJoint.targetVelocity = targetVelocity;
    }

    public void SetConfigurableJoint(
        JointProjectionMode jointProjectionMode,
        RotationDriveMode rotationDriveMode,
        bool enablePreprocessing)
    {
        if (!_configurableJoint)
            return;

        _configurableJoint.projectionMode = jointProjectionMode;
        _configurableJoint.enablePreprocessing = enablePreprocessing;
        _configurableJoint.rotationDriveMode = rotationDriveMode;
    }

    public void SetRigidbody(
        float mass,
        float drag,
        float angularDrag,
        CollisionDetectionMode collisionDetectionMode,
        RigidbodyConstraints rigidbodyConstraints,
        bool isKinematic)
    {
        _rigidbody.mass = mass;
        _rigidbody.linearDamping = drag;
        _rigidbody.angularDamping = angularDrag;
        _rigidbody.collisionDetectionMode = collisionDetectionMode;
        _rigidbody.constraints = rigidbodyConstraints;
        _rigidbody.isKinematic = isKinematic;
    }

    public void SetPos(Vector3 pos, float deltaTime, float AddForce)
    {
        if (!CurrentPosState || !_configurableJoint)
            return;

        Vector3 newPos = Vector3.Lerp(_configurableJoint.targetPosition, pos, deltaTime);
        _configurableJoint.targetPosition = newPos;
        _rigidbody.AddForce((pos - transform.position).normalized * AddForce);
    }

    public void SetRot(Quaternion rot, float speed)
    {
        if (!CurrentRotState || !_configurableJoint)
            return;

        Quaternion newRot = _configurableJoint.SetTargetRotationLocal(rot, _cachedStartRot);

        _configurableJoint.targetRotation =
            Quaternion.Lerp(_configurableJoint.targetRotation, newRot, speed * Time.fixedDeltaTime);
    }

    public void SetPos(Vector3 pos)
    {
        if (!CurrentPosState || !_configurableJoint)
            return;

        _configurableJoint.targetPosition = pos;
        _rigidbody.position = pos;
    }
    
    public void SetRot(Quaternion rot)
    {
        if (!CurrentRotState || !_configurableJoint)
            return;

        Quaternion newRot = _configurableJoint.SetTargetRotationLocal(rot, _cachedStartRot);

        _configurableJoint.targetRotation = newRot;
        //_rigidbody.rotation = newRot;
    }

    public void SetPositionDrive(float value)
    {
        JointDrive drive = new JointDrive
        {
            maximumForce = _configurableJoint.xDrive.maximumForce,
            positionSpring = Mathf.Clamp(value, 0, int.MaxValue),
            positionDamper = _configurableJoint.xDrive.positionDamper,
            useAcceleration = _configurableJoint.xDrive.useAcceleration
        };
        
        _configurableJoint.angularXDrive = drive;
        _configurableJoint.angularYZDrive = drive;

        _configurableJoint.xDrive = drive;
        _configurableJoint.yDrive = drive;
        _configurableJoint.zDrive = drive;
    }

    public void IsPositionApplying(bool isPositionApplying) => CurrentPosState = isPositionApplying;

    public void IsRotationApplying(bool isRotationApplying) => CurrentRotState = isRotationApplying;
}