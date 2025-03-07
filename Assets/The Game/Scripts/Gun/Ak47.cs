using System;
using Shadow_Dominion.InputSystem;
using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonobehaviourInitializer
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

        private IInputHandler _monoInputHandler;
        private WeaponSO _weaponSo;
        private Transform _lookTarget;
        private Transform _transform;
        private RaycastHit _hit;

        public void Construct(
            IInputHandler monoInputHandler,
            Transform lookTarget,
            WeaponSO weaponSo)
        {
            _monoInputHandler = monoInputHandler;
            _lookTarget = lookTarget;
            _weaponSo = weaponSo;
            _transform = transform;

            _monoInputHandler.OnInputUpdate += Fire;

            IsInitialized = true;
        }

        private void OnDestroy()
        {
            if (_monoInputHandler == null)
                return;
            
            _monoInputHandler.OnInputUpdate -= Fire;
        }

        private void Fire(InputData inputData)
        {
            if (!inputData.LeftMouseButton)
                return;

            OnFired?.Invoke(bulletStartPosition.position, transform.forward * _weaponSo.Damage);

            if (fireEffect.isPlaying)
                fireEffect.Stop();
            fireEffect.Play();
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
                return;
            
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

        private void OnDrawGizmos()
        {
            Debug.DrawLine(bulletStartPosition.position, _hit.point, Color.red);
        }
    }
}