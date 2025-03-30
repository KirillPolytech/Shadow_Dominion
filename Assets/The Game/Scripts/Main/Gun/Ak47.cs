using System;
using Shadow_Dominion.InputSystem;
using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonoBehaviour
    {
        private const int Distance = 1000;
        private const float FullRotation = 360f;
        private const float HalfRotation = 180f;

        public Action<Vector3, Vector3> OnFired;

        [SerializeField]
        private Transform bulletStartPosition;

        [SerializeField]
        private Transform weaponPose;

        [SerializeField]
        private ParticleSystem fireEffect;
        
        public Vector3 HitPoint => _hit.point;
        public Vector3 BulletStartPosition => bulletStartPosition.position;
        public Transform InitialParent { get; private set; }

        private WeaponSO _weaponSo;
        private Transform _lookTarget;
        private Transform _transform;
        private RaycastHit _hit;
        
        private Vector3 _initialPos;
        private Quaternion _initialRot;
        
        private Vector3 _ragdollPos;
        private Quaternion _ragdollRot;

        private const float _fireDelay = 0.1f;
        private float _fireDelayTimer;

        public void Construct(
            Transform lookTarget,
            WeaponSO weaponSo)
        {
            _lookTarget = lookTarget;
            _weaponSo = weaponSo;
            _transform = transform;
            InitialParent = transform.parent;

            _initialPos = transform.position;
            _initialRot = transform.rotation;

            _ragdollPos = new Vector3(0.298f, -0.185f, 0.12f);
            _ragdollRot = Quaternion.Euler(new Vector3(329.8f, 247.7f, 207.16f));
        }

        public void HandleInput(InputData inputData)
        {
            RotateTo();
            CastRay();
            
            _fireDelayTimer = Mathf.Clamp(_fireDelayTimer + Time.deltaTime, 0 , _fireDelay + 1);
            
            if (!inputData.LeftMouseButton || _fireDelayTimer < _fireDelay)
                return;

            _fireDelayTimer = 0;
            
            OnFired?.Invoke(bulletStartPosition.position, transform.forward * _weaponSo.Damage);

            if (fireEffect.isPlaying)
                fireEffect.Stop();
            fireEffect.Play();
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
            Quaternion targetLocalRotation = Quaternion.LookRotation(_lookTarget.position - _transform.position);

            weaponPose.rotation = Quaternion.Lerp(_transform.rotation, targetLocalRotation,
                _weaponSo.RotationSpeed * Time.fixedDeltaTime);

            float lookAxisX = weaponPose.localEulerAngles.x > HalfRotation 
                ? weaponPose.localEulerAngles.x - FullRotation : weaponPose.localEulerAngles.x;
            
            float lookAxisY = weaponPose.localEulerAngles.y > HalfRotation 
                ? weaponPose.localEulerAngles.y - FullRotation : weaponPose.localEulerAngles.y;
            float lookAxisZ = weaponPose.localEulerAngles.z;

            lookAxisX = Mathf.Clamp(lookAxisX, -_weaponSo.Limit, _weaponSo.Limit);
            lookAxisY = Mathf.Clamp(lookAxisY, -_weaponSo.Limit, _weaponSo.Limit);
            
            weaponPose.localEulerAngles = new Vector3(lookAxisX, lookAxisY, lookAxisZ);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        
        public void SetInitialTransform()
        {
            transform.localPosition = _initialPos;
            transform.localRotation = _initialRot;
        }

        public void SetRagdollTransform()
        {
            transform.localPosition = _ragdollPos;
            transform.localRotation = _ragdollRot;
        }
        
        private void OnDrawGizmos()
        {
            Debug.DrawLine(bulletStartPosition.position, _hit.point, Color.red);
        }
    }
}