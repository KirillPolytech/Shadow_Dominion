using UnityEngine;

public class PIDController
{
    // A PID controller
    public static Vector3 PIDControl(float P, float D, Vector3 error, ref Vector3 lastError)
    {
        // signal = P * (error + D * derivative)
        Vector3 signal = P * (error + D * (error - lastError) / Time.fixedDeltaTime);
        lastError = error;
        return signal;
    }
}
