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

        public void IsPositionApplying(bool isPositionApplying) => CurrentPosState = isPositionApplying;
        public void IsRotationApplying(bool isRotationApplying) => CurrentRotState = isRotationApplying;

        public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

        public BoneSettings BoneSettings;

        private Quaternion _cachedStartRot;
        private float _cachedInitialPositionSpring, _cachedPositionDamper;
        private ConfigurableJoint _configurableJoint;
        private Rigidbody _rigidbody;
        private Transform _copyTarget;
        private PIDData _pidData;
        private SpringData _springData;

        private float _springRate = 1;

        private Vector3 _previousError;
        private Renderer _renderer;

        public void Construct(
            SpringData springData, 
            Transform copyTarget, 
            PIDData pidData, 
            Renderer skinnedMeshRenderer)
        {
            _springData = springData;
            _copyTarget = copyTarget;
            _pidData = pidData;
            _renderer = skinnedMeshRenderer;
        }

        private void Awake()
        {
            _configurableJoint = GetComponent<ConfigurableJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            BoneSettings = new BoneSettings(_configurableJoint, _rigidbody);
            _cachedStartRot = transform.localRotation;
            _cachedInitialPositionSpring = _configurableJoint.xDrive.positionSpring;
            _cachedPositionDamper = _configurableJoint.xDrive.positionDamper;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateConfigurableJoint();
        }

        private void UpdatePosition()
        {
            if (!CurrentPosState)
                return;

            Vector3 error = _copyTarget.position - _rigidbody.worldCenterOfMass;
            _rigidbody.AddForce(
                PIDController.PIDControl(_pidData.PForce, _pidData.DForce, error, ref _previousError),
                ForceMode.VelocityChange);

            Debug.DrawLine(CurrentPosition, _copyTarget.position, Color.blue);
        }

        private void UpdateConfigurableJoint()
        {
            if (!CurrentPosState || !CurrentRotState)
                return;
            
            _configurableJoint.targetPosition = _copyTarget.position;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(_copyTarget.localRotation, _cachedStartRot);
            _configurableJoint.targetRotation = newRot;
        }

        private void UpdatePositionSpring(float value)
        {
            JointDrive drive = new JointDrive
            {
                maximumForce = _configurableJoint.xDrive.maximumForce,
                positionSpring = Mathf.Clamp(value, 0, _cachedInitialPositionSpring),
                positionDamper = Mathf.Clamp(value, 0, value / (_cachedInitialPositionSpring - _cachedPositionDamper)),
                useAcceleration = _configurableJoint.xDrive.useAcceleration
            };

            _configurableJoint.angularXDrive = drive;
            _configurableJoint.angularYZDrive = drive;

            _configurableJoint.xDrive = drive;
            _configurableJoint.yDrive = drive;
            _configurableJoint.zDrive = drive;
        }

        public void AddForce(Vector3 dir)
        {
            _rigidbody.AddForce(dir);
            
            _springRate = Mathf.Clamp(_springRate - 1f, 0f, 1);

            UpdatePositionSpring(CurrentPositionSpring * _springRate);
        }

        public void ReceiveDamage(Vector3 dir)
        {
            OnCollision?.Invoke(dir);
        }

        public void ReceiveHitPoint(Vector3 point)
        {
            BodyInjuryService.DrawHole(_renderer, point);
        }

        private void OnCollisionStay(Collision other)
        {
            if (!CurrentPosState)
                return;
            
            if (!other.gameObject.CompareTag(TagStorage.Obstacle))
                return;

            Vector3 dir = (transform.position - other.transform.position).normalized;
            OnCollision?.Invoke(dir);
        }
    }
}