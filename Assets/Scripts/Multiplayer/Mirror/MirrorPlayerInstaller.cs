using HellBeavers;
using HellBeavers.Player;
using UnityEngine;
using Zenject;

public class MirrorPlayerInstaller : MonoBehaviour
{
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

    [Space] [Header("PlayerHeight")] [SerializeField]
    private Transform height;

    [SerializeField] private Transform target;
    [SerializeField] private PlayerHeightData playerHeightData;

    [Header("Factories")] [SerializeField]
    private Bullet bulletPrefab;

    private BulletPool _bulletPool;

    [Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    private void Awake()
    {
        cameraLook.Construct(cameraSettings);
        aimTarget.Construct(cameraLook);
        playerMovement.Construct(playerSettings, charRigidbody, cameraLook, inputHandler, legPlacer);
        playerAnim.Construct(animator, inputHandler);
        
        ak47.Construct(_bulletPool);
    }
}