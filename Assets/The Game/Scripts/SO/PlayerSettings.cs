using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = PathStorage.ScriptableObjectMenu + "/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float WalkSpeed = 6.1f;
    public float RunSpeed = 7;
    [Space]
    public float MaxWalkSpeed;
    public float MaxRunSpeed;
    
    public float JumpForce = 20;
    
    [Space]
    public float RotSpeed = 5;
    public float AimRigWeightChange = 3.5f;
    
    [Space]
    public float Approximation = 0.01f;
    
    [Space, Range(0,1000)]
    public int RagdollDelay = 3;
    
    [Space, Range(0,10)]
    public int MoveAnimToRagdollTime = 3;

    [Space, Range(0, 1)] 
    public float StopDistance = 0.25f;
        
    public string[] FallRayMask = {"Ragdoll", "Animation"};
    public string ShootMask = "Animation";
    
    public float GroundCheckDistance = 1.5f;
}