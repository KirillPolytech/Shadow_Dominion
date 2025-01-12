using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Shadow_Dominion
{
    public class ZombieInstaller : MonoBehaviour
    {
        [SerializeField] private ZombieSettings zombieSettings;
        [SerializeField] private ZombieMovement zombieMovement;
        [SerializeField] private ZombieTargetDetector zombieTargetDetector;

        [Space] [Header("Motion")] [SerializeField]
        private SpringData springData;

        [SerializeField] private Transform anim;
        [SerializeField] private Transform[] copyFrom;
        [SerializeField] private BoneController[] copyTo;
        [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;

        private void Awake()
        {
            zombieMovement.Construct(zombieSettings);
            
            zombieTargetDetector.OnDetectTarget += zombieMovement.MoveTo;

            for (int i = 0; i < copyFrom.Length; i++)
                copyTo[i].Construct(springData, copyFrom[i]);
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