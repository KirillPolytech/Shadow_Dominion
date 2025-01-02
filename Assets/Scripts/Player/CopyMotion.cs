using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField] private Rigidbody stabilizer;
    [SerializeField] private Transform hips;
    
    [SerializeField] private Transform anim;
    [SerializeField] private Transform[] copyFrom;
    [SerializeField] private BoneController[] copyTo;

    [SerializeField] private bool isCopyPos;
    [SerializeField] private bool isCopyRot;
    [SerializeField] private bool isDeactivable;

    [Range(0, 1f)] [SerializeField] private float deactivateDistance = 0.1f;
    [Range(0, 0.5f)] [SerializeField] private float sphereRadius = 0.1f;
    [Range(0, 16)] [SerializeField] private int enabledBones = 1;

    //[SerializeField] private float strengthPos = 2500;
    //[SerializeField] private float strengthRot = 1500;
    //[SerializeField] private float CopyStrength = 1;

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

    public void SmoothDeactivate(bool isDeactivate)
    {
        if (!isDeactivate)
            return;
        
        for (int i = 0; i < enabledBones; i++)
        {
            float newVal = copyTo[i].CurrentPositionSpring - 50;
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

    public void IsCopyPos(bool isCopy) => isCopyPos = isCopy;
    public void IsCopyRot(bool isCopy) => isCopyRot = isCopy;

    private void HandleBonesPosition()
    {
        if (!isCopyPos)
            return;

        for (int i = 0; i < enabledBones; i++)
        {
            copyTo[i].IsPositionApplying(true);

            //Vector3 copyFromPos = copyFrom[i].position;
            //float value = CopyStrength * strengthPos * Time.fixedDeltaTime;
            //copyTo[i].SetPos( copyFromPos, value, value);
            //copyTo[i].SetPos(copyFrom[i].position);
            stabilizer.position = hips.position;
            stabilizer.rotation = hips.rotation;

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