using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class ZombieInstaller : MonoBehaviour
    {
        [SerializeField] private BoneDataSO boneDataSo;
        [SerializeField] private Zombie zombie;
        [SerializeField] private ZombieSettings zombieSettings;
        [SerializeField] private ZombieMovement zombieMovement;
        [SerializeField] private ZombieTargetDetector zombieTargetDetector;
        [SerializeField] private Animator animator;
        [SerializeField] private PIDData pidData;
        [SerializeField] private Renderer rend;

        [Space] [Header("Motion")] [SerializeField]
        private SpringData springData;

        [SerializeField] private Transform anim;
        [SerializeField] private Transform[] copyFrom;
        [SerializeField] private BoneController[] copyTo;
        [Range(0, 0.5f)] [SerializeField] private float sphereRadius;

        private void Awake()
        {
            zombieMovement.Construct(animator, zombieSettings);

            zombieTargetDetector.OnDetectTarget += zombieMovement.MoveTo;

            for (int i = 0; i < copyFrom.Length; i++)
            {
                copyTo[i].Construct(springData, copyFrom[i], pidData, rend, boneDataSo.BoneData[i].humanBodyBone);

                copyTo[i].OnCollision += dir => zombie.Disable(copyTo, dir);
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
            zombieTargetDetector.OnDetectTarget -= zombieMovement.MoveTo;
        }
    }
}