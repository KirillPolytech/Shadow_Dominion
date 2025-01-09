using System.Linq;
using HellBeavers;
using HellBeavers.Player;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class MirrorPlayerInstaller : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private AimTarget aimTarget;
    [SerializeField] private Ak47 ak47;
    [SerializeField] private MonoInputHandler inputHandler;

    [Space] [SerializeField] private Animator animator;
    [SerializeField] private CameraSettings cameraSettings;

    [Header("PlayerMovement")] [SerializeField]
    private PlayerSettings playerSettings;

    [SerializeField] private Rigidbody charRigidbody;
    [SerializeField] private LegPlacer legPlacer;
    
    private BulletPool _bulletPool;

    [Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    private void Awake()
    {
        player.Construct(inputHandler);
        cameraLook.Construct(cameraSettings);
        aimTarget.Construct(cameraLook);
        playerMovement.Construct(playerSettings, charRigidbody, cameraLook, inputHandler, legPlacer);
        playerAnim.Construct(animator, inputHandler);
        ak47.Construct(_bulletPool);
        
        for (int i = 0; i < copyFrom.Length; i++)
            copyTo[i].Construct(inputHandler, springData, copyFrom[i]);
    }
    
    [Space][Header("Motion")]
    [SerializeField] private SpringData springData;
    [SerializeField] private Transform anim;
    [SerializeField] private Transform[] copyFrom;
    [SerializeField] private BoneController[] copyTo;

    [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;
    
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