using UnityEngine;

public class BoneSettings
{
    private readonly ConfigurableJoint _configurableJoint;
    private readonly Rigidbody _rigidbody;

    public BoneSettings(ConfigurableJoint configurableJoint, Rigidbody rigidbody)
    {
        _configurableJoint = configurableJoint;
        _rigidbody = rigidbody;
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
        float angularXYZMaxForce,
        float angularXYZDrivePositionSpring,
        float angularXYZDrivePositionDamper,
        bool useRotAcceleration)
    {
        JointDrive drive = new JointDrive
        {
            maximumForce = posMaximumForce,
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            useAcceleration = usePosAcceleration
        };

        JointDrive angularXDrive = new JointDrive
        {
            maximumForce = angularXYZMaxForce,
            positionSpring = angularXYZDrivePositionSpring,
            positionDamper = angularXYZDrivePositionDamper,
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
        _configurableJoint.targetPosition = targetPosition;

        _configurableJoint.targetRotation = targetRotation;
        
        _configurableJoint.targetVelocity = targetVelocity;
    }

    public void SetConfigurableJoint(
        JointProjectionMode jointProjectionMode,
        RotationDriveMode rotationDriveMode,
        bool enablePreprocessing)
    {
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
}