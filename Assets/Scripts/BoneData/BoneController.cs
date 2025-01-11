using UnityEngine;

namespace Shadow_Dominion
{
    public class BoneController : MonoBehaviour
    {
        public Vector3 CurrentPosition => _rigidbody.position;
        public Quaternion CurrentRotation => _rigidbody.rotation;

        public bool CurrentPosState { get; private set; } = true;
        public bool CurrentRotState { get; private set; } = true;

        public void IsPositionApplying(bool isPositionApplying) => CurrentPosState = isPositionApplying;
        public void IsRotationApplying(bool isRotationApplying) => CurrentRotState = isRotationApplying;

        public float CurrentPositionSpring => _configurableJoint.xDrive.positionSpring;

        public BoneSettings BoneSettings;

        private Quaternion _cachedStartRot;
        private float _cachedInitialPositionSpring;
        private ConfigurableJoint _configurableJoint;
        private Rigidbody _rigidbody;
        private SpringData _springData;
        private Transform _copyTarget;

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
        }

        private void UpdatePosition()
        {
            if (!CurrentPosState)
                return;

            _configurableJoint.targetPosition = _copyTarget.position;
            _rigidbody.position = Vector3.Lerp(_rigidbody.position, _copyTarget.position, Time.fixedDeltaTime * _springData.Rate);
            
            Debug.DrawLine(CurrentPosition, _copyTarget.position, Color.blue);
        }

        private void UpdateRotation()
        {
            if (!CurrentRotState)
                return;

            Quaternion newRot = _configurableJoint.SetTargetRotationLocal(_copyTarget.localRotation, _cachedStartRot);
            _configurableJoint.targetRotation = newRot;
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

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Obstacle")) //!other.gameObject.CompareTag("Bullet") ||
                return;

            UpdatePositionSpring(CurrentPositionSpring - _cachedInitialPositionSpring / 2);
        }
    }
}