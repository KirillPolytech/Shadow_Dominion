using UnityEngine;

public static class ConfigurableJointExtensions
{
    /// <summary>
    /// Sets a joint's targetRotation to match a given local rotation.
    /// The joint transform's local rotation must be cached on Start and passed into this method.
    /// </summary>
    public static Quaternion SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation,
        Quaternion startLocalRotation)
    {
        if (joint.configuredInWorldSpace)
        {
            Debug.LogError(
                "SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.",
                joint);
        }

        return SetTargetRotationInternal(joint, targetLocalRotation, startLocalRotation, Space.Self);
    }

    /// <summary>
    /// Sets a joint's targetRotation to match a given world rotation.
    /// The joint transform's world rotation must be cached on Start and passed into this method.
    /// </summary>
    public static Quaternion SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation,
        Quaternion startWorldRotation)
    {
        if (!joint.configuredInWorldSpace)
        {
            Debug.LogError(
                "SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.",
                joint);
        }

        return SetTargetRotationInternal(joint, targetWorldRotation, startWorldRotation, Space.World);
    }

    private static Quaternion SetTargetRotationInternal(ConfigurableJoint joint, Quaternion targetRotation,
        Quaternion startRotation, Space space)
    {
        // Calculate the rotation expressed by the joint's axis and secondary axis
        Vector3 right = joint.axis;
        Vector3 forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion worldToJointSpace = Quaternion.identity;
        if (Vector3.Cross(forward, up) != Vector3.zero)
            worldToJointSpace = Quaternion.LookRotation(forward, up);

        // Transform into world space
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        // Counter-rotate and apply the new local rotation.
        // Joint space is the inverse of world space, so we need to invert our value
        if (space == Space.World)
        {
            resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
        }
        else
        {
            resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
        }

        // Transform back into joint space
        resultRotation *= worldToJointSpace;

        // Set target rotation to our newly calculated rotation
        return resultRotation;
    }
}