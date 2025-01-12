using System.Linq;
using NaughtyAttributes;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorPlayerInstaller : MonoBehaviour
    {
        [SerializeField] private Main.Player player;
        [SerializeField] private MonoInputHandler inputHandler;
        [SerializeField] private PlayerAnim playerAnim;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraLook cameraLook;
        [SerializeField] private AimTarget aimTarget;

        [Space] [Header("Gun")] [SerializeField]
        private Ak47 ak47;

        [SerializeField] private Transform aim;

        [Space] [SerializeField] private Animator animator;
        [SerializeField] private CameraSettings cameraSettings;

        [Header("PlayerMovement")] [SerializeField]
        private PlayerSettings playerSettings;

        [SerializeField] private Rigidbody charRigidbody;
        [SerializeField] private LegPlacer legPlacer;

        [Space] [Header("Motion")] [SerializeField]
        private SpringData springData;

        [SerializeField] private Transform anim;
        [SerializeField] private Transform[] copyFrom;
        [SerializeField] private BoneController[] copyTo;
        [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;


        private BulletPool _bulletPool;

        [Inject]
        public void Construct(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
        }

        private void Awake()
        {
            //player.Construct(inputHandler);
            cameraLook.Construct(cameraSettings, inputHandler);
            aimTarget.Construct(cameraLook);
            playerMovement.Construct(playerSettings, charRigidbody, cameraLook, inputHandler, legPlacer);
            playerAnim.Construct(animator, inputHandler);
            ak47.Construct(inputHandler, _bulletPool, aim);

            for (int i = 0; i < copyFrom.Length; i++)
            {
                copyTo[i].Construct(springData, copyFrom[i]);

                int ind = i;
                inputHandler.OnInputUpdate += inp => HandleInput(inp, copyTo[ind]);
            }

            return;

            void HandleInput(InputData inputData, BoneController boneController)
            {
                boneController.IsPositionApplying(!inputData.E);
                boneController.IsRotationApplying(!inputData.E);
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