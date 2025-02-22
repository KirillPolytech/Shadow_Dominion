
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = PathStorage.ScriptableObjectMenu + "/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float zoom = 1;
    public float zoomDuration = 5;
    public float zoomInDistance = 0.5f;
    
    [Range(0,10)]public float maxZoomDistance = 5;
    
    public int rayCastDistance = 1000;
    public LayerMask layerMask;
}
