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
        private Transform _copyTarget;
        private PIDData _pidData;
        private SpringData _springData;

        private float _springRate = 1;

        private Vector3 _previousError;

        public void Construct(SpringData springData, Transform copyTarget, PIDData pidData)
        {
            _springData = springData;
            _copyTarget = copyTarget;
            _pidData = pidData;
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
            UpdateConfigurableJoint();
            UpdatePosition();
            UpdateRotation();
            UpdateFreezee();
        }

        private void UpdateConfigurableJoint()
        {
            _configurableJoint.targetPosition = _copyTarget.position;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(_copyTarget.localRotation, _cachedStartRot);
            _configurableJoint.targetRotation = newRot;
        }

        private void UpdatePosition()
        {
            if (!CurrentPosState)
                return;

            //if (_springData.Rate != 0)
            //transform.position = Vector3.Lerp(transform.position, _copyTarget.position, Time.fixedDeltaTime * _springRate * _springData.Rate);

            //_rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, _copyTarget.position, Time.fixedDeltaTime * _springData.Rate * _springRate));

            Vector3 error = _copyTarget.position - _rigidbody.worldCenterOfMass;
            _rigidbody.linearVelocity =
                PIDController.PIDControl(_pidData.PForce, _pidData.DForce, error, ref _previousError);

            Debug.DrawLine(CurrentPosition, _copyTarget.position, Color.blue);
        }

        private void UpdateRotation()
        {
            if (!CurrentRotState)
                return;

            //if (_springData.Rate != 0)
            //transform.rotation = Quaternion.Lerp(transform.rotation, _copyTarget.rotation, Time.fixedDeltaTime * _springRate * _springData.Rate);

            //_rigidbody.MoveRotation(_copyTarget.rotation);
        }

        private void UpdateFreezee()
        {
            //_rigidbody.constraints = CurrentFreeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
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

            Vector3 dir = (other.transform.position - transform.position).normalized * 20;
            OnCollision?.Invoke(dir);

            _springRate = Mathf.Clamp(_springRate - 0.5f, 0.1f, 1);

            UpdatePositionSpring(CurrentPositionSpring * _springRate);
        }
    }
}