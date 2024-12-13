using HellBeavers;
using UnityEngine;

public class Ak47 : MonoBehaviour
{
    [SerializeField] private Transform aim;
    [SerializeField] private Transform bulletStartPosition;
    [SerializeField] private float bulletVelocity = 100;
    
    public Vector3 BulletStartPos => bulletStartPosition.position;
    public Vector3 HitPoint => _hit.point;

    private BulletPool _bulletPool;
    private readonly int _distance = 1000;
    private RaycastHit _hit;

    //[Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) 
            return;
        
        Bullet bullet = _bulletPool.Pull();
        bullet.Initialize(
            bulletStartPosition.position, 
            Quaternion.LookRotation(transform.forward), 
            transform.forward * bulletVelocity);
    }

    private void FixedUpdate()
    {
        RotateTo();
        CastRay();
    }

    private void CastRay()
    {
        Physics.Raycast(BulletStartPos, transform.forward, out _hit, _distance);
    }

    private void RotateTo()
    {
        transform.rotation = Quaternion.LookRotation(aim.position - transform.position);
    }   

    private void OnDrawGizmos()
    {
        Debug.DrawLine(BulletStartPos, _hit.point, Color.red);
    }
}