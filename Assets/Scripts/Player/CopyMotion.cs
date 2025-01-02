using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField] private Transform anim;
    [SerializeField] private Transform[] copyFrom;
    [SerializeField] private BoneController[] copyTo;

    [SerializeField] private float strengthPos = 1;
    [SerializeField] private float strengthRot = 1;
    [SerializeField] private float CopyStrength;

    [SerializeField] private bool isCopyPos;
    [SerializeField] private bool isCopyRot;
    [SerializeField] private bool isDeactivable;
    
    [Range(0, 1f)] [SerializeField] 
    private float deactivateDistance = 0.1f;
    [Range(0, 0.5f)] [SerializeField] 
    private float sphereRadius = 0.1f;
    [Range(0, 16)] [SerializeField] 
    private int enabledBones = 1;
    
    [Button]
    public void Initialize()
    {
        copyTo = transform.GetComponentsInChildren<BoneController>().ToArray();

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
    }

    private void HandleBonesPosition()
    {
        if (!isCopyPos)
            return;
        
        for (int i = 0; i < enabledBones; i++)
        {
            if ((copyTo[i].CurrentPosition - copyFrom[i].position).magnitude > deactivateDistance && isDeactivable)
            {
                copyTo[i].IsPositionApplying(false);
                continue;
            }

            copyTo[i].IsPositionApplying(true);

            Vector3 copyFromPos = copyFrom[i].position;
            float value = CopyStrength * strengthPos * Time.fixedDeltaTime;
            //copyTo[i].SetPos( copyFromPos, value, value);
            copyTo[i].SetPos( copyFromPos);
                
            Debug.DrawLine(copyTo[i].CurrentPosition, copyFromPos, Color.blue);
        }
    }

    private void HandleBonesRotation()
    {
        if (!isCopyRot)
            return;
        
        for (int i = 0; i < enabledBones; i++)
        {
            if ((copyTo[i].CurrentPosition - copyFrom[i].position).magnitude > deactivateDistance && isDeactivable)
            {
                copyTo[i].IsRotationApplying(false);
                continue;
            }
            
            copyTo[i].IsRotationApplying(true);
            //copyTo[i].SetRot(copyFrom[i].localRotation, CopyStrength * strengthRot);
            copyTo[i].SetRot(copyFrom[i].localRotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < copyFrom.Length; i++)
        {
            Gizmos.DrawSphere(copyFrom[i].position, sphereRadius);
        }

        return;
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
    }
}