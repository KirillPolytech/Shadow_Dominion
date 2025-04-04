using UnityEngine;

namespace Shadow_Dominion
{
    public class GunLineDirectionVisualizer : MonoBehaviour
    {
        [SerializeField] private LineRenderer prefab;
        [Range(0, 1)] [SerializeField] private float size = 0.005f;

        private Ak47 _ak47;
        private LineRenderer _lineRend;
        private Vector3 _lastHitPos;

        private void Start()
        {
            _lineRend = Instantiate(prefab);
            _lineRend.positionCount = 2;
            _lineRend.startWidth = size;
            _lineRend.endWidth = size;

            _ak47 = GetComponent<Ak47>();
        }

        private void FixedUpdate()
        {
            DrawLine();
        }

        private void DrawLine()
        {
            if (_ak47.HitPoint == _lastHitPos)
            {
                _lineRend.SetPosition(0, _ak47.BulletStartPosition);
                _lineRend.SetPosition(1, _ak47.BulletStartPosition);
                return;
            }            
            _lineRend.SetPosition(0, _ak47.BulletStartPosition);
            _lineRend.SetPosition(1, _ak47.HitPoint);

            _lastHitPos = _ak47.HitPoint;
        }
    }
}