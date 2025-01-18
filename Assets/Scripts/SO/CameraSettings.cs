
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "HellBeaversData/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float zoom = 1;
    public float zoomDuration = 5;
    
    public int rayCastDistance = 1000;
    public LayerMask layerMask;
}
