using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "HellBeaversData/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float speed = 7500;
    public float rotSpeed = 100;
    [Range(-1, 1)] public float tilt = -0.1f;
    
    [Space] 
    public bool canMove = true;
    public bool canRotate = true;
    public bool canPlaceLegs = true;
}