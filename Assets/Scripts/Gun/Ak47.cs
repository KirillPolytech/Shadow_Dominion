using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonoBehaviour
    {
        private const int Distance = 1000;

        [SerializeField] private Transform bulletStartPosition;
        [SerializeField] private Transform weaponPose;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private ParticleSystem fireEffect;

        public Vector3 HitPoint => _hit.point;
        public Vector3 BulletStartPosition => bulletStartPosition.position;

        private MonoInputHandler _monoInputHandler;
        private RaycastHit _hit;
        private Transform _lookTarget;
        private Transform _cachedTransform;

        //[Inject]
        public void Construct(MonoInputHandler monoInputHandler, Transform lookTarget)
        {
            _monoInputHandler = monoInputHandler;
            _lookTarget = lookTarget;
            _cachedTransform = transform;

            _monoInputHandler.OnInputUpdate += Fire;
        }

        private void Fire(InputData inputData)
        {
            if (!inputData.LeftMouseButtonDown)
                return;
            
            fireEffect.Play();
            
            if (!_hit.collider)
                return;
            
            BoneController boneController = _hit.collider.GetComponent<BoneController>();

            if (!boneController)
                return;

            boneController.ReceiveDamage((boneController.CurrentPosition - _cachedTransform.position) * 10000);
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