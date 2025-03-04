
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = PathStorage.ScriptableObjectMenu + "/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float ZoomSpeed = 0.1f;
    [Space]    
    public float cameraMinDistance;
    public float cameraMaxDistance;
    [Space]
    public int rayCastDistance = 1000;
    public LayerMask layerMask;
}
