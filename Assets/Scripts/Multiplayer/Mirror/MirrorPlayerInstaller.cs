using System;
using System.Linq;
using NaughtyAttributes;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.StateMachine;
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
        private MonoInputHandler monoInputHandler;

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
        private Rigidbody ragdollRoot;

        [Space]
        [SerializeField]
        private CoroutineExecuter coroutineExecuter;

        [Space]
        [Header("Network")]
        [SerializeField]
        private MirrorShootHandler mirrorShootHandler;

        [SerializeField]
        private MirrorStateHandler mirrorStateHandler;

        [Space]
        [SerializeField]
        private AnimationClip standUpFaceUpClip;
        [SerializeField]
        private AnimationClip standUpFaceDownClip;


        [Space]
        [Header("Debug")]
        [SerializeField]
        private bool debug;

        private Action<Vector3> _cachedOnCollision;
        private Action<HumanBodyBones> _cachedOnBoneDetached;
        private Action<InputData> _cachedInputData;

        private void Awake()
        {
            PlayerMovement playerMovement = new PlayerMovement();
            PlayerAnimation playerAnimation = new PlayerAnimation();
            PlayerStateMachine playerStateMachine = new PlayerStateMachine(
                player,
                cameraLook,
                ragdollRoot.transform,
                playerAnimation,
                rootRig,
                copyTo,
                coroutineExecuter,
                playerMovement,
                monoInputHandler,
                standUpFaceUpClip,
                standUpFaceDownClip);

            cameraLook.Construct(cameraSettings, monoInputHandler, cinemachineThirdPersonFollow);
            aimTarget.Construct(cameraLook);
            playerAnimation.Construct(animator, aimRig, ragdollRoot, playerStateMachine);
            playerMovement.Construct(playerSettings, charRigidbody, cameraLook, playerAnimation);
            mirrorStateHandler.Construct(playerStateMachine);
            ak47.Construct(monoInputHandler, aim, mirrorShootHandler);

            for (int i = 0; i < copyFrom.Length; i++)
            {
                HumanBodyBones humanBodyBone = bones.BoneData.First(x => x.Name == copyTo[i].name).humanBodyBone;

                copyTo[i].Construct(springData, copyFrom[i], pidData, rend, humanBodyBone);

                int ind = i;
                _cachedInputData = inp => HandleInput(inp, copyTo[ind]);
                monoInputHandler.OnInputUpdate += _cachedInputData;

                _cachedOnCollision = deltaDist => OnCollision(deltaDist, ind, playerStateMachine);
                copyTo[i].OnCollision += _cachedOnCollision;
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

        private void OnCollision(Vector3 deltaDist, int ind, IStateMachine playerStateMachine)
        {
            if (copyTo[ind].BoneType == HumanBodyBones.RightLowerArm
                || copyTo[ind].BoneType == HumanBodyBones.RightUpperArm
                || copyTo[ind].BoneType == HumanBodyBones.LeftLowerArm
                || copyTo[ind].BoneType == HumanBodyBones.LeftUpperArm)
            {
                aimRig.weight = 0;
            }

            if (copyTo[ind].BoneType == HumanBodyBones.LeftLowerLeg
                || copyTo[ind].BoneType == HumanBodyBones.LeftUpperLeg
                || copyTo[ind].BoneType == HumanBodyBones.RightUpperLeg
                || copyTo[ind].BoneType == HumanBodyBones.RightLowerLeg)
            {
                playerStateMachine.SetState<RagdollState>();
                return;
            }

            copyTo[ind].IsPositionApplying(false);
            copyTo[ind].IsRotationApplying(false);
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

            //Gizmos.DrawRay(ragdollRoot.position, ragdollRoot.transform.up * 5);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < copyFrom.Length; i++)
            {
                copyTo[i].OnCollision -= _cachedOnCollision;
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