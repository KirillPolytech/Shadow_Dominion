using UnityEngine;

[CreateAssetMenu(fileName = "PIDData", menuName = PathStorage.ScriptableObjectMenu + "/PIDData")]
public class PIDData : ScriptableObject
{
    [Header("Proportional force")] [Range(0f, 160f)]
    public float PForce = 8f;

    [Header("Derivative force")] [Range(0f, 0.064f)]
    public float DForce = 0.01f;
}