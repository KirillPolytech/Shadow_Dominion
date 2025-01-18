using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "HellBeaversData/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float walkSpeed = 9;
    public float runSpeed = 12;
    public float rotSpeed = 5;
    [Range(-1, 1)] public float tilt = -0.1f;
    
    [Space] 
    public bool canMove = true;
    public bool canRotate = true;
    public bool canPlaceLegs = true;
}