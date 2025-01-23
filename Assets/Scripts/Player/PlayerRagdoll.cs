using System.Linq;
using Shadow_Dominion.Player;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Shadow_Dominion
{
    public class PlayerRagdoll : MonoBehaviour
    {
        private const float StandUpDelay = 7.07f;
        private const float ResetBonesTime = 1.5f;

        [SerializeField] private PhysicsMaterial mat;
        [SerializeField] private Animator animator;
        [SerializeField] private Shadow_Dominion.Main.Player player;
        [SerializeField] private CharacterController characterController;

        private CharacterJoint[] _joints;
        private Collider[] _cols;
        private Rigidbody[] _rbs;

        private float _delay;
        private bool _isStandUp;
        private Transform[] _bones;
        private BoneTransform[] _standUpBoneTransforms;
        private BoneTransform[] _ragDollBoneTransforms;
        private Transform _hipBone;

        private float _resetDelayCounter;
        private float _value;

        private void Awake()
        {
            _joints = GetComponentsInChildren<CharacterJoint>();
            _cols = GetComponentsInChildren<Collider>();
            _rbs = GetComponentsInChildren<Rigidbody>();

            _hipBone = animator.GetBoneTransform(HumanBodyBones.Hips);
            _bones = _hipBone.GetComponentsInChildren<Transform>();

            foreach (var joint in _joints)
            {
                joint.enableProjection = true;

                joint.lowTwistLimit = new SoftJointLimit { limit = -10 };
                joint.highTwistLimit = new SoftJointLimit { limit = 10 };
                joint.swing1Limit = new SoftJointLimit { limit = 20 };
                joint.swing2Limit = new SoftJointLimit { limit = 20 };
            }

            foreach (var col in _cols)
            {
                col.material = mat;
            }

            foreach (var rb in _rbs)
            {
                rb.isKinematic = true;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }

        private void Update()
        {
            SpaceHandler();
            EHandler();

            if (!_isStandUp)
                return;

            if (_resetDelayCounter == 0)
            {
                _hipBone.SetParent(null);
                transform.position = _hipBone.position;
                _hipBone.position = transform.position;
                _hipBone.SetParent(transform);

                PopulateBoneTransforms(_ragDollBoneTransforms);
                PopulateStandUpBones(_standUpBoneTransforms);

                //AlignPosToHips();
                //AlignRotToHips();
            }

            HandleSmoothMovementToStandUpPos();

            if (_value < 1)
                return;

            if (_rbs[0].isKinematic == false)
            {
                foreach (var rb in _rbs)
                {
                    rb.isKinematic = true;
                }
            }

            animator.enabled = true;

            _delay += Time.deltaTime;

            if (_delay < StandUpDelay)
                return;

            _isStandUp = false;

            player.enabled = true;
            characterController.enabled = true;
        }

        private void SpaceHandler()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
                return;

            foreach (var rb in _rbs)
            {
                rb.isKinematic = false;
                rb.linearVelocity = -transform.forward * Random.Range(0, 25);
            }

            animator.enabled = false;
            player.enabled = false;
            characterController.enabled = false;
            _delay = 0;
            _value = 0;
            _resetDelayCounter = 0;
        }

        private void HandleSmoothMovementToStandUpPos()
        {
            if (_value >= 0.999f)
                return;

            _resetDelayCounter += Time.deltaTime;
            float percent = _resetDelayCounter / ResetBonesTime;
            _value = Mathf.Clamp(percent, 0, 1);
            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition =
                    Vector3.Lerp(_ragDollBoneTransforms[i].pos, _standUpBoneTransforms[i].pos, _value);
                _bones[i].localRotation =
                    Quaternion.Lerp(_ragDollBoneTransforms[i].rot, _standUpBoneTransforms[i].rot, _value);
            }
        }

        private void EHandler()
        {
            if (!Input.GetKeyDown(VariableNames.E))
                return;

            foreach (var rb in _rbs)
            {
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rb.isKinematic = true;
            }

            _standUpBoneTransforms = new BoneTransform[_bones.Length];
            _ragDollBoneTransforms = new BoneTransform[_bones.Length];

            for (int i = 0; i < _bones.Length; i++)
            {
                _standUpBoneTransforms[i] = new BoneTransform();
                _ragDollBoneTransforms[i] = new BoneTransform();
            }

            //_playerAnimation.AnimationStateMachine.SetState<StandupState>();

            _isStandUp = true;
        }

        private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
        {
            for (int i = 0; i < _bones.Length; i++)
            {
                boneTransforms[i].pos = _bones[i].localPosition;
                boneTransforms[i].rot = _bones[i].localRotation;
            }
        }

        private void PopulateStandUpBones(BoneTransform[] boneTransforms)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips.First(x => x.name == "Standup");

            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            clip.SampleAnimation(gameObject, 0);

            PopulateBoneTransforms(boneTransforms);

            transform.position = pos;
            transform.rotation = rot;

            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition = _ragDollBoneTransforms[i].pos;
                _bones[i].localRotation = _ragDollBoneTransforms[i].rot;
            }
        }

        private void AlignPosToHips()
        {
            Vector3 hipsPos = _hipBone.position;

            Vector3 posOffset = _standUpBoneTransforms[0].pos;
            posOffset.y = 0;
            posOffset = transform.rotation * posOffset;
            transform.position -= posOffset;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }

            _hipBone.position = hipsPos;
        }

        private void AlignRotToHips()
        {
            Vector3 desDir = -_hipBone.up;
            desDir.y = 0;
            desDir = desDir.normalized;

            Quaternion fromTo = Quaternion.FromToRotation(transform.forward, desDir);
            transform.rotation *= fromTo;
        }
    }

    public struct BoneTransform
    {
        public Vector3 pos;
        public Quaternion rot;
    }
}