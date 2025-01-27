using System;
using System.Linq;
using NaughtyAttributes;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion
{
    public class MirrorPlayerInstaller : MonoBehaviour
    {
        [Space]
        [Header("Configs")]
        [SerializeField]
        private BoneDataSO bones;

        [SerializeField]
        private SpringData springData;

        [SerializeField]
        private PIDData pidData;

        [SerializeField]
        private CameraSettings cameraSettings;

        [SerializeField]
        private PlayerSettings playerSettings;

        [Space]
        [Header("Limits")]
        [SerializeField]
        private Main.Player player;

        [SerializeField]
        private MonoInputHandler inputHandler;

        [SerializeField]
        private PlayerAnimation playerAnimation;

        [SerializeField]
        private PlayerMovement playerMovement;

        [SerializeField]
        private CameraLook cameraLook;

        [SerializeField]
        private AimTarget aimTarget;

        [SerializeField]
        private CinemachineThirdPersonFollow cinemachineThirdPersonFollow;

        [SerializeField]
        private Renderer rend;

        [Space]
        [Header("Gun")]
        [SerializeField]
        private Ak47 ak47;

        [SerializeField]
        private Transform aim;

        [Space]
        [SerializeField]
        private Animator animator;

        [Header("PlayerMovement")]
        [SerializeField]
        private Rigidbody charRigidbody;

        [Space]
        [Header("Motion")]
        [SerializeField]
        private Transform anim;

        [SerializeField]
        private Transform[] copyFrom;

        [SerializeField]
        private BoneController[] copyTo;

        [Range(0, 0.5f)]
        [SerializeField]
        private float sphereRadius = 0.1f;

        [Space]
        [Header("Rig")]
        [SerializeField]
        private RigBuilder rootRig;

        [SerializeField]
        private Rig aimRig;

        [Space]
        [Header("StateMachine")]
        [SerializeField]
        private Transform ragdollRoot;

        [Space]
        [Header("Debug")]
        [SerializeField]
        private bool debug;

        private Action<Vector3> _cachedV3;
        private Action<HumanBodyBones> _cachedHBB;
        private Action<InputData> _cachedInputData;

        private void Awake()
        {
            player.Construct(ragdollRoot, rootRig, playerMovement, playerAnimation, cameraLook, copyTo, inputHandler);
            cameraLook.Construct(cameraSettings, inputHandler, cinemachineThirdPersonFollow);
            aimTarget.Construct(cameraLook);
            playerMovement.Construct(playerSettings, charRigidbody, cameraLook, inputHandler);
            playerAnimation.Construct(animator, inputHandler, aimRig, player.playerStateMachine);
            ak47.Construct(inputHandler, aim);

            for (int i = 0; i < copyFrom.Length; i++)
            {
                HumanBodyBones humanBodyBone = bones.BoneData.First(x => x.Name == copyTo[i].name).humanBodyBone;

                copyTo[i].Construct(springData, copyFrom[i], pidData, rend, humanBodyBone);

                int ind = i;
                _cachedInputData = inp => HandleInput(inp, copyTo[ind]);
                inputHandler.OnInputUpdate += inp => HandleInput(inp, copyTo[ind]);

                _cachedV3 = dir => player.playerStateMachine.SetState<RagdollState>();
                copyTo[i].OnCollision += _cachedV3;

                _cachedHBB = dir => player.playerStateMachine.SetState<RagdollState>();
                copyTo[i].OnBoneDetach += _cachedHBB;
            }

            return;

            void HandleInput(InputData inputData, BoneController boneController)
            {
                if (!debug)
                    return;

                boneController.IsPositionApplying(!inputData.T);
                boneController.IsRotationApplying(!inputData.T);
            }
        }

        [Button("InitializeMotion")]
        public void Initialize()
        {
            copyTo = GetComponentsInChildren<BoneController>().ToArray();

            Transform[] transforms = new Transform[copyTo.Length];
            Transform[] animTransforms = anim.GetComponentsInChildren<Transform>();

            for (int i = 0; i < copyTo.Length; i++)
                transforms[i] = animTransforms.First(x => x.name == copyTo[i].name);

            copyFrom = transforms;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            foreach (var t in copyFrom)
            {
                Gizmos.DrawSphere(t.position, sphereRadius);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < copyFrom.Length; i++)
            {
                copyTo[i].OnCollision -= _cachedV3;
                copyTo[i].OnBoneDetach -= _cachedHBB;
            }
        }
    }
}

/*
GUIStyle style = new GUIStyle { normal = { textColor = Color.red } };

foreach (var t in copyFrom)
{
    Handles.Label(t.localPosition, $"x:{t.localPosition.x:#.00} y:{t.localPosition.y:#.00} z:{t.localPosition.z:#.00}", style);
}

foreach (var t in copyFrom)
{
    Handles.Label(t.position, $"x:{t.position.x:#.00} y:{t.position.y:#.00} z:{t.position.z:#.00}", style);
}
*/