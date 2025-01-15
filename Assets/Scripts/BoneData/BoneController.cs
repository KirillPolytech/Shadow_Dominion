using System;
using UnityEngine;

namespace Shadow_Dominion
{
    public class BoneController : MonoBehaviour
    {
        public event Action<Vector3> OnCollision;

        public Vector3 CurrentPosition => _rigidbody.position;
        public Quaternion CurrentRotation => _rigidbody.rotation;

        public bool CurrentPosState { get; private set; } = true;
        public bool CurrentRotState { get; private set; } = true;
        public bool CurrentFreeze { get; private set; } = true;

        public void IsPositionApplying(bool isPositionApplying) => CurrentPosState = isPositionApplying;
        public void IsRotationApplying(bool isRotationApplying) => CurrentRotState = isRotationApplying;
        public void IsFreezeed(bool isFreezeed) => CurrentFreeze = isFreezeed;

        public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

        public BoneSettings BoneSettings;

        private Quaternion _cachedStartRot;
        private float _cachedInitialPositionSpring;
        private ConfigurableJoint _configurableJoint;
        private Rigidbody _rigidbody;
        private SpringData _springData;
        private Transform _copyTarget;

        private float _springRate = 1;
        private Vector3 integralError;
        private Vector3 previousError;

        public void Construct(SpringData springData, Transform copyTarget)
        {
            _springData = springData;
            _copyTarget = copyTarget;
        }

        private void Awake()
        {
            _configurableJoint = GetComponent<ConfigurableJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            BoneSettings = new BoneSettings(_configurableJoint, _rigidbody);
            _cachedStartRot = transform.localRotation;
            _cachedInitialPositionSpring = _configurableJoint.xDrive.positionSpring;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateRotation();
            UpdateFreezee();
        }

        private void UpdatePosition()
        {
            if (!CurrentPosState)
                return;

            _configurableJoint.targetPosition = _copyTarget.position;

            //if (gameObject.name is "mixamorig:RightUpLeg" or "mixamorig:LeftUpLeg" or "mixamorig:LeftLeg" or "mixamorig:RightLeg")

            //_rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, _copyTarget.position, Time.fixedDeltaTime * _springData.Rate * _springRate));
            if (_springData.Rate != 0)
                transform.position = Vector3.Lerp(transform.position, _copyTarget.position,
                    Time.fixedDeltaTime * _springRate * _springData.Rate);

            Debug.DrawLine(CurrentPosition, _copyTarget.position, Color.blue);
        }

        private void UpdateRotation()
        {
            if (!CurrentRotState)
                return;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(_copyTarget.localRotation, _cachedStartRot);
            _configurableJoint.targetRotation = newRot;

            if (_springData.Rate != 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, _copyTarget.rotation,
                    Time.fixedDeltaTime * _springRate * _springData.Rate);
            //_rigidbody.rotation = _copyTarget.rotation;
        }

        private void UpdateFreezee()
        {
            _rigidbody.constraints = CurrentFreeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        }

        private void UpdatePositionSpring(float value)
        {
            JointDrive drive = new JointDrive
            {
                maximumForce = _configurableJoint.xDrive.maximumForce,
                positionSpring = Mathf.Clamp(value, 0, _cachedInitialPositionSpring),
                positionDamper = _configurableJoint.xDrive.positionDamper,
                useAcceleration = _configurableJoint.xDrive.useAcceleration
            };

            _configurableJoint.angularXDrive = drive;
            _configurableJoint.angularYZDrive = drive;

            _configurableJoint.xDrive = drive;
            _configurableJoint.yDrive = drive;
            _configurableJoint.zDrive = drive;
        }
        
        public void AddForce(Vector3 dir) => _rigidbody.AddForce(dir);

        public void ReceiveDamage(Vector3 dir)
        {
            OnCollision?.Invoke(dir);
        }

        private void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.CompareTag("Obstacle"))
                return;

            Vector3 dir = other.transform.position - transform.position;
            OnCollision?.Invoke(dir);

            _springRate = Mathf.Clamp(_springRate - 0.5f, 0.1f, 1);

            UpdatePositionSpring(CurrentPositionSpring * _springRate);
        }
    }
}