using HellBeavers;
using UnityEngine;

public class Ak47 : MonoBehaviour
{
    [SerializeField] private Transform aim;
    [SerializeField] private Transform bulletStartPosition;
    [SerializeField] private float bulletVelocity = 100;

    private BulletPool _bulletPool;

    //[Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet bullet = _bulletPool.GetFree();
            bullet.Initialize(
                bulletStartPosition.position, 
                Quaternion.LookRotation(transform.forward), 
                transform.forward * bulletVelocity);
        }
    }

    private void FixedUpdate()
    {
        RotateTo();
    }

    private void RotateTo()
    {
        transform.rotation = Quaternion.LookRotation(aim.position - transform.position);
    }   

    private void OnDrawGizmos()
    {
        Debug.DrawRay(bulletStartPosition.position, -transform.forward * bulletVelocity, Color.blue);
    }
}