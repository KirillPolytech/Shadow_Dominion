using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonoBehaviour
    {
        private const int Distance = 1000;

        [SerializeField] private Transform bulletStartPosition;
        [SerializeField] private Transform weaponPose;
        [SerializeField] private float damage = 100;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private float limit = 30;
        [SerializeField] private ParticleSystem fireEffect;

        public Vector3 HitPoint => _hit.point;
        public Vector3 BulletStartPosition => bulletStartPosition.position;

        private MonoInputHandler _monoInputHandler;
        private RaycastHit _hit;
        private Transform _lookTarget;
        private Transform _cachedTransform;

        public void Construct(MonoInputHandler monoInputHandler, Transform lookTarget)
        {
            _monoInputHandler = monoInputHandler;
            _lookTarget = lookTarget;
            _cachedTransform = transform;

            _monoInputHandler.OnInputUpdate += Fire;
        }

        private void Fire(InputData inputData)
        {
            if (!inputData.LeftMouseButton)
                return;
            
            if (fireEffect.isPlaying)
                fireEffect.Stop();
            fireEffect.Play();
            
            if (!_hit.collider)
                return;
            
            BoneController boneController = _hit.collider.GetComponent<BoneController>();

            if (!boneController)
                return;

            boneController.ReceiveDamage((boneController.CurrentPosition - _cachedTransform.position) * damage);
            //boneController.ReceiveHitPoint(_hit.point);
        }

        private void FixedUpdate()
        {
            RotateTo();
            CastRay();
        }

        private void CastRay()
        {
            Ray ray = new Ray(bulletStartPosition.position, transform.forward);
            Physics.Raycast(ray, out _hit, Distance);

            if (_hit.point == default)
            {
                _hit.point = ray.GetPoint(Distance);
            }
        }

        private void RotateTo()
        {
            weaponPose.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(_lookTarget.position - transform.position),
                rotationSpeed * Time.fixedDeltaTime);
            
            Vector3 euler = weaponPose.rotation.eulerAngles;
            euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
            euler.x = Mathf.Clamp(euler.x, -limit, limit);
            
            weaponPose.rotation = Quaternion.Euler(euler);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(bulletStartPosition.position, _hit.point, Color.red);
        }

        private void OnDisable()
        {
            _monoInputHandler.OnInputUpdate -= Fire;
        }
    }
}