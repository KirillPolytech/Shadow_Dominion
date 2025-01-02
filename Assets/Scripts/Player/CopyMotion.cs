using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace HellBeavers
{
    public class CopyMotion : MonoBehaviour
    {
        [SerializeField] private Transform anim;
        [SerializeField] private Transform[] copyFrom;
        [SerializeField] private BoneController[] copyTo;

        [SerializeField] private bool isCopyPos;
        [SerializeField] private bool isCopyRot;
        [SerializeField] private bool isDeactivable;

        [Range(0, 1f)] [SerializeField] private float deactivateDistance = 0.1f;
        [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;
        [Range(0, 16)] [SerializeField] private int enabledBones = 1;
        
        [Range(0, 100)] [SerializeField] private int springDelta = 50;
        
        [SerializeField]private float rate = 10; 

        private float _deltaTime = 100;
        private readonly float _deltaMax = 100;

        public void IsCopyPos(bool isCopy) => isCopyPos = isCopy;
        public void IsCopyRot(bool isCopy) => isCopyRot = isCopy;
        
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

        private void FixedUpdate()
        {
            HandleBonesPosition();
            HandleBonesRotation();
            HandleBonesDeactivation();
        }

        private void UpdateDeltaTime(bool isDeactivate)
        {
            if (isDeactivate)
            {
                _deltaTime = Mathf.Clamp(_deltaTime - Time.fixedDeltaTime * rate, 0, _deltaMax);
                return;
            }
            
            _deltaTime = Mathf.Clamp(_deltaTime + Time.fixedDeltaTime * rate,  0, _deltaMax);
        }

        public void SmoothDeactivate(bool isDeactivate)
        {
            UpdateDeltaTime(isDeactivate);
            
            if (!isDeactivate)
            {
                for (int i = 0; i < enabledBones; i++)
                {
                    float newVal = copyTo[i].CurrentPositionSpring + springDelta;
                    copyTo[i].SetPositionDrive(newVal);
                }
                return;
            }

            for (int i = 0; i < enabledBones; i++)
            {
                float newVal = copyTo[i].CurrentPositionSpring - springDelta;
                copyTo[i].SetPositionDrive(newVal);
            }
        }

        private void HandleBonesDeactivation()
        {
            if (!isDeactivable)
                return;

            for (int i = 0; i < enabledBones; i++)
            {
                if ((copyTo[i].CurrentPosition - copyFrom[i].position).magnitude > deactivateDistance)
                {
                    copyTo[i].IsPositionApplying(false);
                    copyTo[i].IsRotationApplying(false);
                }
            }
        }

        private void HandleBonesPosition()
        {
            if (!isCopyPos)
                return;
            
            for (int i = 0; i < enabledBones; i++)
            {
                copyTo[i].IsPositionApplying(true);
                copyTo[i].SetPos(copyFrom[i].position, _deltaTime);

                Debug.DrawLine(copyTo[i].CurrentPosition, copyFrom[i].position, Color.blue);
            }
        }

        private void HandleBonesRotation()
        {
            if (!isCopyRot)
                return;

            for (int i = 0; i < enabledBones; i++)
            {
                copyTo[i].IsRotationApplying(true);
                copyTo[i].SetRot(copyFrom[i].localRotation);

                //Debug.DrawRay(copyFrom[i].position, copyFrom[i].forward, Color.black);
                //Debug.DrawRay(copyFrom[i].position, copyTo[i].transform.forward, Color.yellow);
            }
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

//[SerializeField] private float strengthPos = 2500;
//[SerializeField] private float strengthRot = 1500;
//[SerializeField] private float CopyStrength = 1;

//Vector3 copyFromPos = copyFrom[i].position;
//float value = CopyStrength * strengthPos * Time.fixedDeltaTime;
//copyTo[i].SetPos( copyFromPos, value, value);


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