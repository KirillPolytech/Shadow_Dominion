using UnityEngine;

[CreateAssetMenu(fileName = "PIDData", menuName = PathStorage.ScriptableObjectMenu + "/PIDData")]
public class PIDData : ScriptableObject
{
    [Tooltip("Proportional force of PID controller.")] [Range(0f, 160f)]
    public float PForce = 8f;

    [Tooltip("Derivative force of PID controller.")] [Range(0f, 0.064f)]
    public float DForce = 0.01f;
}