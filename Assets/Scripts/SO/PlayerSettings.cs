using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = PathStorage.ScriptableObjectMenu + "/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float walkSpeed = 9;
    public float runSpeed = 12;
    public float rotSpeed = 5;
    public float aimRigWeightChange = 3.5f;

    [Space]
    public bool canMove = true;

    public bool canRotate = true;

    [Space]
    public float Approximation = 0.01f;
}