using System;
using Shadow_Dominion.InputSystem;
using UnityEngine;

namespace Shadow_Dominion
{
    public class Ak47 : MonoBehaviour
    {
        private const int Distance = 1000;

        public Action<Vector3, Vector3> OnFired;

        [SerializeField] private Transform bulletStartPosition;
        [SerializeField] private Transform weaponPose;
        [SerializeField] private ParticleSystem fireEffect;

        public Vector3 HitPoint => _hit.point;
        public Vector3 BulletStartPosition => bulletStartPosition.position;

        private IInputHandler _monoInputHandler;
        private WeaponSO _weaponSo;
        private RaycastHit _hit;
        private Transform _lookTarget;

        public void Construct(
            IInputHandler monoInputHandler, 
            Transform lookTarget, 
            WeaponSO weaponSo)
        {
            _monoInputHandler = monoInputHandler;
            _lookTarget = lookTarget;
            _weaponSo = weaponSo;

            _monoInputHandler.OnInputUpdate += Fire;
        }
        
        private void OnDestroy()
        {
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
                _weaponSo.RotationSpeed * Time.fixedDeltaTime);
            
            Vector3 euler = weaponPose.rotation.eulerAngles;
            euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
            euler.x = Mathf.Clamp(euler.x, -_weaponSo.Limit, _weaponSo.Limit);
            
            weaponPose.rotation = Quaternion.Euler(euler);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(bulletStartPosition.position, _hit.point, Color.red);
        }
    }
}