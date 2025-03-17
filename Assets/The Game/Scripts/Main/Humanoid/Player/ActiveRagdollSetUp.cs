using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

namespace Shadow_Dominion
{
    [ExecuteInEditMode]
    public class ActiveRagdollSetUp : MonoBehaviour
    {
        [SerializeField] private Transform root;

        [Space(15)] [Header("Targets")] [SerializeField]
        private Vector3 TargetPosition;

        [SerializeField] private Vector3 TargetVelocity;
        [SerializeField] private Quaternion TargetRotation = Quaternion.identity;
        [SerializeField] private RotationDriveMode rotationDriveMode;

        [Space(15)] [Header("PositionDrive")] [SerializeField]
        private int positionMaxForce = 150000000;

        [SerializeField] private int positionSpringDrive = 1500;
        [SerializeField] private int positionDamper = 25;
        [SerializeField] private bool usePosAcceleration;

        [Space(15)] [Header("RotationDrive")] [SerializeField]
        private int angularMaxForce = 150000000;

        [SerializeField] private int angularPositionSpring = 1500;
        [SerializeField] private int angularPositionDamper = 25;
        [SerializeField] private bool useRotAcceleration;

        [Space(15)] [Header("Limits")] [SerializeField]
        private BoneDataSO bones;

        [Range(0, 100f)] [SerializeField] private float LinearLimit;
        [SerializeField] private float LinearLimitSpring;
        [SerializeField] private float LinearLimitDamper;

        [Space(15)] [Header("MotionStates")] [SerializeField]
        private ConfigurableJointMotion PosMotionState = ConfigurableJointMotion.Locked;

        [SerializeField] private ConfigurableJointMotion RotMotionState = ConfigurableJointMotion.Limited;

        [Space(15)] [Header("Rigidbody")] [Range(0, 100)] [SerializeField]
        private int mass = 1;

        [Range(0, 100)] [SerializeField] private int drag = 1;
        [SerializeField] private bool isKinematic;
        [SerializeField] private CollisionDetectionMode collisionDetectionMode;
        [SerializeField] private RigidbodyConstraints rigidbodyConstraints;
        [SerializeField] private PhysicsMaterial physicMaterial;

        [Space(15)] [Header("Other")] [SerializeField]
        private JointProjectionMode ProjectionMode;

        [SerializeField] private bool enablePreprocessing;

        private void Start()
        {
            try
            {
                UpdateRagdoll();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        [Button]
        public void UpdateValues()
        {
            try
            {
                UpdateRagdoll();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        [Button]
        public void CreateRagdoll()
        {
            CharacterJoint[] joints = root.GetComponentsInChildren<CharacterJoint>();
            for (int i = 0; i < joints.Length; i++)
            {
                Rigidbody rb = joints[i].connectedBody;
                GameObject gb = joints[i].gameObject;

                DestroyImmediate(joints[i]);

                ConfigurableJoint cj = gb.AddComponent<ConfigurableJoint>();
                cj.connectedBody = rb;

                gb.AddComponent<BoneController>();
            }

            Transform[] t = root.GetComponentsInChildren<Transform>();
            for (int i = 0; i < t.Length; i++)
            {
                var x = t[i].GetComponent<ConfigurableJoint>();
                if (x == null)
                    continue;

                if (!t[i].GetComponent<BoneController>())
                    t[i].AddComponent<BoneController>();

                Collider col = t[i].GetComponent<Collider>();
                if (!col)
                {
                    col = t[i].AddComponent<BoxCollider>();
                    ((BoxCollider)col).size = new Vector3(0.1f, 0.1f, 0.1f);
                }else if (col.GetType() == typeof(BoxCollider))
                    ((BoxCollider)col).size = new Vector3(0.1f, 0.1f, 0.1f);

                if (!t[i].GetComponent<ConfigurableJoint>())
                    t[i].AddComponent<ConfigurableJoint>();
            }

            ConfigurableJoint[] configurableJoints = root.GetComponentsInChildren<ConfigurableJoint>();
            for (int i = 0; i < configurableJoints.Length; i++)
            {
                Rigidbody temp = configurableJoints[i].transform.parent.GetComponent<Rigidbody>();
                Transform tr = configurableJoints[i].transform;
                while (temp == null || tr.name == "Root")
                {
                    tr = tr.transform.parent;
                    temp = tr.GetComponent<Rigidbody>();
                }
                configurableJoints[i].connectedBody = temp;
            }
        }

        [Button]
        public void EnableBoneControlles()
        {
            BoneController[] controllers = root.GetComponentsInChildren<BoneController>();

            foreach (var controller in controllers)
                controller.enabled = true;
        }

        [Button]
        public void DisableBoneControlles()
        {
            BoneController[] controllers = root.GetComponentsInChildren<BoneController>();

            foreach (var controller in controllers)
                controller.enabled = false;
        }

        [Button]
        public void DeleteExtra()
        {
            ConfigurableJoint[] joints = root.GetComponentsInChildren<ConfigurableJoint>();

            for (int i = 0; i < joints.Length; i++)
            {
                if (!joints[i])
                    continue;

                ConfigurableJoint[] components = joints[i].GetComponents<ConfigurableJoint>();

                if (components.Length <= 1)
                    continue;

                for (int j = 0; j < components.Length - 1; j++)
                {
                    if (!components[j])
                        continue;

                    DestroyImmediate(components[j]);
                }
            }
        }

        [Button]
        public void DeleteAll()
        {
            ConfigurableJoint[] joints = root.GetComponentsInChildren<ConfigurableJoint>();
            Rigidbody[] rigidbodies = root.GetComponentsInChildren<Rigidbody>();
            Collider[] colliders = root.GetComponentsInChildren<Collider>();

            for (int i = 0; i < joints.Length; i++)
            {
                DestroyImmediate(joints[i]);
            }

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                DestroyImmediate(rigidbodies[i]);
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                DestroyImmediate(colliders[i]);
            }
        }

        [Button]
        public void GCCollect()
        {
            GC.Collect();
            GarbageCollector.CollectIncremental();
        }

        private void UpdateRagdoll()
        {
            BoneController[] controllers = root.GetComponentsInChildren<BoneController>();
            Collider[] colls = root.GetComponentsInChildren<Collider>();

            for (int i = 0; i < bones.BoneData.Length; i++)
            {
                BoneController controller = controllers.FirstOrDefault(x => x.name == bones.BoneData[i].Name);

                if (!controller)
                {
                    Debug.LogWarning($"Can't find bone: {bones.BoneData[i].Name}");
                    continue;
                }

                controller.BoneSettings.SetJointLimits(
                    bones.BoneData[i].angularYLimit,
                    bones.BoneData[i].angularZLimit,
                    bones.BoneData[i].highAngularXLimit,
                    bones.BoneData[i].lowAngularXLimit,
                    LinearLimit,
                    LinearLimitSpring,
                    LinearLimitDamper);

                controller.BoneSettings.SetDrive(
                    positionMaxForce,
                    positionSpringDrive,
                    positionDamper,
                    usePosAcceleration,
                    angularMaxForce,
                    angularPositionSpring,
                    angularPositionDamper,
                    useRotAcceleration
                );

                controller.BoneSettings.SetConfigurableJoint(ProjectionMode, rotationDriveMode, enablePreprocessing);

                controller.BoneSettings.SetPositionMotionState(PosMotionState);
                controller.BoneSettings.SetRotationMotionState(RotMotionState);

                controller.BoneSettings.SetTargets(TargetPosition, TargetRotation, TargetVelocity);

                controller.BoneSettings.SetRigidbody(mass,
                    drag,
                    drag,
                    collisionDetectionMode,
                    rigidbodyConstraints,
                    isKinematic);
            }

            foreach (var col in colls)
            {
                col.material = physicMaterial;
            }
        }
    }
}