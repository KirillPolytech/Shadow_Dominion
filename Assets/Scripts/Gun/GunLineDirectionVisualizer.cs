using UnityEngine;

public class GunLineDirectionVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer prefab;

    private Ak47 _ak47;
    private LineRenderer _lineRend;
    
    private void Start()
    {
        _lineRend = Instantiate(prefab);
        _lineRend.positionCount = 2;

        _ak47 = GetComponent<Ak47>();
    }

    private void FixedUpdate()
    { 
        _lineRend.SetPosition(0, _ak47.BulletStartPos);
        _lineRend.SetPosition(1, _ak47.HitPoint);
    }
}
