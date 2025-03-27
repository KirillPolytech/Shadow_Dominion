using System;
using System.Linq;
using Mirror;
using NaughtyAttributes;
using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.Network;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using WindowsSystem;

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

        [SerializeField]
        private WeaponSO weaponSO;

        [FormerlySerializedAs("player")]
        [Space]
        [Header("Limits")]
        [SerializeField]
        private MirrorPlayer mirrorPlayer;

        [SerializeField]
        private Transform animTransform;

        [SerializeField]
        private CameraLook cameraLook;

        [SerializeField]
        private AimTarget aimTarget;

        [SerializeField]
        private CinemachineOrbitalFollow cinemachinePosition;

        [SerializeField]
        private CinemachineRotationComposer cinemachineRotation;

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
        private Rigidbody AnimRigidbody;

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

        [Space]
        [SerializeField]
        private AnimationClip standUpFaceUpClip;
        
        [SerializeField]
        private AnimationClip standUpFaceDownClip;

        [Space]
        [Header("Debug")]
        [SerializeField]
        private bool debug;

        private Action<Vector3, string> _cachedOnCollision;
        private Action<HumanBodyBones> _cachedOnBoneDetached;
        private Action<InputData> _cachedInputData;

        private void Awake()
        {
            BoneSettings[] boneSettings = new BoneSettings[copyTo.Length];

            for (int i = 0; i < copyTo.Length; i++)
            {
                boneSettings[i] = new BoneSettings(copyTo[i].GetComponent<ConfigurableJoint>(), 
                    copyTo[i].GetComponent<Rigidbody>());
                
                HumanBodyBones humanBodyBone = bones.BoneData.First(x => x.Name == copyTo[i].name).humanBodyBone;

                copyTo[i].Construct(copyFrom[i], pidData, rend, humanBodyBone, boneSettings[i]);
            }
            
            WindowsController windowsController = FindAnyObjectByType<WindowsController>();
            PlayerMovement playerMovement = new PlayerMovement();
            PlayerAnimation playerAnimation = new PlayerAnimation();
            AnimationStateMachine animationStateMachine = new AnimationStateMachine(animator);
            playerAnimation.Construct(animationStateMachine, aimRig, coroutineExecuter, playerSettings);
            PlayerStateMachine playerStateMachine = new PlayerStateMachine(
                mirrorPlayer,
                cameraLook,
                ragdollRoot.transform,
                playerAnimation,
                rootRig,
                copyTo,
                coroutineExecuter,
                playerMovement,
                monoInputHandler,
                standUpFaceUpClip,
                standUpFaceDownClip,
                windowsController,
                ak47,
                playerSettings);

            cameraLook.Construct(cameraSettings, monoInputHandler, cinemachinePosition, cinemachineRotation);
            mirrorPlayer.Construct(animTransform, AnimRigidbody, ragdollRoot.transform, playerStateMachine, cameraLook);
            aimTarget.Construct(cameraLook);
            playerMovement.Construct(playerSettings, AnimRigidbody, cameraLook, playerAnimation);
            ak47.Construct(aim, weaponSO);
            mirrorShootHandler.Construct(ak47);
            
            for (int i = 0; i < copyFrom.Length; i++)
            {
                int ind = i;
                _cachedInputData = inp => HandleInput(inp, copyTo[ind]);
                monoInputHandler.OnInputUpdate += _cachedInputData;

                _cachedOnCollision = (deltaDist, killerName) 
                    => OnCollision(
                        ind, 
                        playerStateMachine, 
                        playerMovement.IsRunning, 
                        killerName, 
                        MirrorPlayersSyncer.Instance.LocalPlayer.Nick);
                
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

        private void OnCollision(int ind, PlayerStateMachine playerStateMachine, bool isRun, string killerName, string victimName)
        {
            if (playerStateMachine.CurrentState.GetType() == typeof(DeathState))
                return;
            
            if (killerName == null && !isRun)
                return;
            
            if (copyTo[ind].BoneType == HumanBodyBones.Head)
            {
                playerStateMachine.SetState<DeathState>();
                KillFeed.Instance.AddFeed(killerName ?? victimName, victimName);
            }

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