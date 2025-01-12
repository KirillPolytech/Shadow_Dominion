using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Initialize(Vector3 pos, Quaternion rot, Vector3 dir)
    {
        _rb.position = pos;
        _rb.rotation = rot;
        _rb.linearVelocity = dir;
        
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
}
