using System;
using UnityEngine;

namespace Shadow_Dominion
{
    public class BoneController : MonoBehaviour
    {
        public event Action<Vector3, string> OnCollision;

        public HumanBodyBones BoneType { get; private set; }

        public Vector3 CurrentPosition => _rigidbody.position;
        public Quaternion CurrentRotation => _rigidbody.rotation;

        public bool CurrentPositionApplyingState { get; private set; } = true;
        public bool CurrentRotationApplyingState { get; private set; } = true;
        public bool CurrentSpringState { get; private set; } = true;

        public void IsPositionApplying(bool isPositionApplying) => CurrentPositionApplyingState = isPositionApplying;
        public void IsRotationApplying(bool isRotationApplying) => CurrentRotationApplyingState = isRotationApplying;
        public void IsSpringApplying(bool isSpringApplying) => CurrentSpringState = isSpringApplying;

        public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

        public BoneSettings BoneSettings;

        private ConfigurableJoint _configurableJoint;
        private Rigidbody _rigidbody;
        private PIDData _pidData;
        private Transform _copyTarget;
        private Quaternion _cachedStartRot;
        private float _cachedInitialPositionSpring, _cachedPositionDamper;
        private float _springRate = 1;
        private Vector3 _previousError;
        private Renderer _renderer;
        private HumanBodyBones _humanBodyBones;

        public void Construct(
            Transform copyTarget,
            PIDData pidData,
            Renderer skinnedMeshRenderer,
            HumanBodyBones humanBodyBones,
            BoneSettings boneSettings)
        {
            _copyTarget = copyTarget;
            _pidData = pidData;
            _renderer = skinnedMeshRenderer;
            _humanBodyBones = humanBodyBones;
            BoneSettings = boneSettings;

            Initialize();
        }

        private void Initialize()
        {
            _configurableJoint = GetComponent<ConfigurableJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            _cachedStartRot = transform.localRotation;
            _cachedInitialPositionSpring = _configurableJoint.xDrive.positionSpring;
            _cachedPositionDamper = _configurableJoint.xDrive.positionDamper;
            BoneType = _humanBodyBones;

            _rigidbody.position = _copyTarget.position;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateConfigurableJoint();
        }

        private void UpdatePosition()
        {
            if (!CurrentPositionApplyingState)
                return;

            Vector3 error = _copyTarget.position - _rigidbody.worldCenterOfMass;
            _rigidbody.AddForce(
                PIDController.PIDControl(_pidData.PForce, _pidData.DForce, error, ref _previousError),
                ForceMode.VelocityChange);

            Debug.DrawLine(CurrentPosition, _copyTarget.position, Color.blue);
        }

        private void UpdateConfigurableJoint()
        {
            if (!CurrentSpringState)
                return;

            _configurableJoint.targetPosition = _copyTarget.position;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(_copyTarget.localRotation, _cachedStartRot);
            _configurableJoint.targetRotation = newRot;
        }

        public void HasSpring(bool state)
        {
            _springRate = Mathf.Clamp(_springRate - 1f * (state ? -1 : 1), 0f, 1);

            float value = _cachedInitialPositionSpring * _springRate;

            BoneSettings.SetDrive(
                _configurableJoint.xDrive.maximumForce,
                Mathf.Clamp(value, 0, _cachedInitialPositionSpring),
                Mathf.Clamp(value, 0, value / (_cachedInitialPositionSpring - _cachedPositionDamper)),
                _configurableJoint.xDrive.useAcceleration,
                _configurableJoint.angularXDrive.maximumForce,
                value,
                value,
                _configurableJoint.angularXDrive.useAcceleration
            );
        }

        public void AddForce(Vector3 dir) => _rigidbody.AddForce(dir);
        public void ReceiveHitDirection(Vector3 dir) => BodyInjuryService.DrawHole(_renderer, dir);
        public void ReceiveDamage(Vector3 dir, string nick) => OnCollision?.Invoke(dir, nick);

        private void OnCollisionStay(Collision other)
        {
            if (!CurrentPositionApplyingState)
                return;

            if (!other.gameObject.CompareTag(TagStorage.Obstacle))
                return;

            Vector3 dir = other.transform.position - transform.position;
            OnCollision?.Invoke(dir, null);
        }
    }
}