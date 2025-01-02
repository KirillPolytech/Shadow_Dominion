using System.Collections;
using DitzelGames.FastIK;
using NaughtyAttributes;
using UnityEngine;

public class LegPlacer : MonoBehaviour
{
    [SerializeField] private bool IsEnabled;
    
    [SerializeField] private int distance = 10;
    [Range(0, 5)][SerializeField] private float stepDelay = 0.1f;
    
    [Range(0.1f, 0.2f)][SerializeField] private float offset = 0.1f;

    [SerializeField] private AnimationCurve heightCurve;

    [Space] [Header("LFoot")]
    [SerializeField] private Transform LFootPos;
    [SerializeField] private BoneController LFoot;
    [SerializeField] private BoneController LLeg;
    [SerializeField] private BoneController LUpLeg;
    [SerializeField] private Transform LRayPosition;
    
    [Space] [Header("RFoot")]
    [SerializeField] private Transform RFootPos;
    [SerializeField] private BoneController RFoot;
    [SerializeField] private BoneController RLeg;
    [SerializeField] private BoneController RUpLeg;
    [SerializeField] private Transform RRayPosition;

    [SerializeField] private LayerMask layer;

    [SerializeField] private FastIKFabric[] fabrics;

    private Transform _playerTransform;
    private Quaternion _initialRotL, _initialRotR;
    private Vector3 _initalForward;
    private bool _isMakingStep, _isLeftFoot;
    
    public void Construct(Transform playerTransform)
    {
        _playerTransform = playerTransform;

        _initialRotL = LFootPos.localRotation;
        _initialRotR = RFootPos.localRotation;

        _initalForward = Vector3.forward;
    }

    [Button]
    public void Initialize()
    {
        fabrics = FindObjectsByType<FastIKFabric>(FindObjectsSortMode.None);
    }
    
    [Button]
    public void EnableFabrics()
    {
        foreach (var fastIKFabric in fabrics)
        {
            fastIKFabric.enabled = true;
        }
    }

    [Button]
    public void DisableFabrics()
    {
        foreach (var fastIKFabric in fabrics)
        {
            fastIKFabric.enabled = false;
        }
    }

    public void Step()
    {
        Physics.Raycast(LRayPosition.position, Vector3.down, out RaycastHit hit, distance, layer);
        Physics.Raycast(RRayPosition.position, Vector3.down, out RaycastHit hit2, distance, layer);

        if (!IsEnabled)
            return;

        if (hit.point == default)
            return;

        Vector3 lookDir = _playerTransform.forward;
        lookDir.y = 0;
            
        Vector3 lFootNewPos = hit.point + new Vector3(0, offset, 0);
        Vector3 rFootNewPos = hit2.point + new Vector3(0, offset, 0);

        Quaternion targetRot = Quaternion.Euler(LFootPos.eulerAngles.x, Quaternion.LookRotation(lookDir).eulerAngles.y,
            LFootPos.eulerAngles.z);

        if (!_isMakingStep)
            StartCoroutine(_isLeftFoot
                ? MakeStep(LFootPos, LFoot, LLeg, LUpLeg, lFootNewPos, targetRot)
                : MakeStep(RFootPos, RFoot, RLeg, RUpLeg, rFootNewPos, targetRot));

        Debug.DrawLine(LRayPosition.position, hit.point, Color.red);
    }

    private IEnumerator MakeStep(
        Transform footPos, 
        BoneController foot, 
        BoneController leg, 
        BoneController upLeg
        ,Vector3 newPos, 
        Quaternion rot)
    {
        _isMakingStep = true;
        
        float curveVal = 0;
        const float length = 0.15f;
        float delayValue = Time.fixedDeltaTime / stepDelay;
        while (curveVal < length)
        {
            footPos.localPosition = new Vector3(footPos.localPosition.x, 
                heightCurve.Evaluate(curveVal), footPos.localPosition.z);
            footPos.rotation = rot;
            
            SetLegPos();
            
            curveVal += delayValue;
            yield return new WaitForFixedUpdate();
        }

        curveVal = 0;
        while (curveVal < 1)
        {
            footPos.localPosition = Vector3.Lerp(footPos.localPosition, newPos, curveVal);

            SetLegPos();

            curveVal += delayValue;
            yield return new WaitForFixedUpdate();
        }
        
        _isLeftFoot = !_isLeftFoot;

        _isMakingStep = false;
        
        yield break;

        void SetLegPos()
        {
            //foot.SetPos( footPos.position, 1, 1);
            //foot.SetRot( footPos.rotation, 1);

            //leg.SetPos(foot.CurrentPosition + Vector3.up * 0.1f, 1, 1);
            //leg.SetRot(footPos.rotation, 1);
            
            //upLeg.SetPos(leg.CurrentPosition + Vector3.up * 0.1f, 1, 1);
            //upLeg.SetRot(footPos.rotation, 1);
        }
    }
}