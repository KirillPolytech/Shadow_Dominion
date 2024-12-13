using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform pos;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private float velocity;
    
    private void Update()
    {   
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dir = (cameraLook.HitPoint - cameraLook.transform.position).normalized * velocity;
            var gmb = Instantiate(prefab, pos.position, pos.rotation);
            gmb.GetComponent<Rigidbody>().AddForce(dir);
        }
    }
}
