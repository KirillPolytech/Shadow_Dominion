using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonoBehaviour
    {
        private const int Distance = 1000;

        [SerializeField] private Transform bulletStartPosition;
        [SerializeField] private float bulletVelocity = 100;
        [SerializeField] private float rotationSpeed = 15;

        public Vector3 BulletStartPos => bulletStartPosition.position;
        public Vector3 HitPoint => _hit.point;

        private BulletPool _bulletPool;
        private MonoInputHandler _monoInputHandler;
        private RaycastHit _hit;
        private Transform _aim;

        //[Inject]
        public void Construct(MonoInputHandler monoInputHandler, BulletPool bulletPool, Transform aim)
        {
            _monoInputHandler = monoInputHandler;
            _bulletPool = bulletPool;
            _aim = aim;

            _monoInputHandler.OnInputUpdate += HandleInput;
        }

        private void HandleInput(InputData inputData)
        {
            if (!inputData.LeftMouseButtonDown)
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
            Ray ray = new Ray(BulletStartPos, transform.forward);
            Physics.Raycast(ray, out _hit, Distance);

            if (_hit.point == default)
            {
                _hit.point = ray.GetPoint(Distance);
            }
        }

        private void RotateTo()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(_aim.position - transform.position), rotationSpeed * Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(BulletStartPos, _hit.point, Color.red);
        }

        private void OnDisable()
        {
            _monoInputHandler.OnInputUpdate -= HandleInput;
        }
    }
}