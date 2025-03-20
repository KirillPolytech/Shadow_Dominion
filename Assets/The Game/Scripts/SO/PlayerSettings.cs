using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = PathStorage.ScriptableObjectMenu + "/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float WalkSpeed = 6.1f;
    public float RunSpeed = 7;
    [Space]
    public float MaxWalkSpeed;
    public float MaxRunSpeed;
    [Space]
    public float RotSpeed = 5;
    public float AimRigWeightChange = 3.5f;

    [Space]
    public bool CanMove = true;

    public bool CanRotate = true;

    [Space]
    public float Approximation = 0.01f;
    
    [Space, Range(0,1000)]
    public int RagdollDelay = 3;
}