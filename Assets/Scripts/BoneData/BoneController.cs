using System;
using UnityEngine;

namespace HellBeavers
{
    public class BoneController : MonoBehaviour
    {
        public Action<BoneController> ActionOnCollisionEnter;
        
        public Vector3 CurrentPosition => _rigidbody.position;
        public Quaternion CurrentRotation => _rigidbody.rotation;

        public bool CurrentPosState { get; private set; } = true;
        public bool CurrentRotState { get; private set; } = true;

        public void IsPositionApplying(bool isPositionApplying) => CurrentPosState = isPositionApplying;
        public void IsRotationApplying(bool isRotationApplying) => CurrentRotState = isRotationApplying;

        public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

        public BoneSettings BoneSettings;

        private Quaternion _cachedStartRot;
        private float _cachedCurrentPositionSpring;
        private ConfigurableJoint _configurableJoint;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _configurableJoint = transform.GetComponent<ConfigurableJoint>();
            _rigidbody = transform.GetComponent<Rigidbody>();
            BoneSettings = new BoneSettings(_configurableJoint, _rigidbody);
            _cachedStartRot = transform.localRotation;
            _cachedCurrentPositionSpring = _configurableJoint.xDrive.positionSpring;
        }

        public void SetPos(Vector3 pos, float deltaTime)
        {
            if (!CurrentPosState || !_configurableJoint)
                return;

            _configurableJoint.targetPosition = pos;
            _rigidbody.position = Vector3.Lerp(_rigidbody.position, pos, Time.fixedDeltaTime * deltaTime);
        }

        public void SetRot(Quaternion rot)
        {
            if (!CurrentRotState || !_configurableJoint)
                return;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(rot, _cachedStartRot);

            _configurableJoint.targetRotation = newRot;
        }

        public void UpdatePositionSpring(float value)
        {
            JointDrive drive = new JointDrive
            {
                maximumForce = _configurableJoint.xDrive.maximumForce,
                positionSpring = Mathf.Clamp(value, 0, _cachedCurrentPositionSpring),
                positionDamper = _configurableJoint.xDrive.positionDamper,
                useAcceleration = _configurableJoint.xDrive.useAcceleration
            };

            _configurableJoint.angularXDrive = drive;
            _configurableJoint.angularYZDrive = drive;

            _configurableJoint.xDrive = drive;
            _configurableJoint.yDrive = drive;
            _configurableJoint.zDrive = drive;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Bullet") || !other.gameObject.CompareTag("Obstacle"))
                return;
            
            ActionOnCollisionEnter?.Invoke(this);
        }
    }
}