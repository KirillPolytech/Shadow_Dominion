
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "HellBeaversData/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public int rayCastDistance = 1000;
    public LayerMask layerMask;
}
