using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace HellBeavers
{
    public class CopyMotion : MonoBehaviour
    {
        [SerializeField] private MonoInputHandler monoInputHandler;
        [SerializeField] private SpringData springData;
        [SerializeField] private Transform anim;
        [SerializeField] private Transform[] copyFrom;
        [SerializeField] private BoneController[] copyTo;

        [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;

        [Button]
        public void Initialize()
        {
            copyTo = GetComponentsInChildren<BoneController>().ToArray();

            Transform[] transforms = new Transform[copyTo.Length];
            Transform[] animTransforms = anim.GetComponentsInChildren<Transform>();

            for (int i = 0; i < copyTo.Length; i++)
                transforms[i] = animTransforms.First(x => x.name == copyTo[i].name);

            copyFrom = transforms;
        }

        private void Awake()
        {
            for (int i = 0; i < copyFrom.Length; i++)
                copyTo[i].Construct(monoInputHandler, springData, copyFrom[i]);
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