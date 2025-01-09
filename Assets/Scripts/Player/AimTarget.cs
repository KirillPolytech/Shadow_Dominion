using UnityEngine;

namespace HellBeavers
{
    public class AimTarget : MonoBehaviour
    {
        [SerializeField] private Transform aim;

        private CameraLook _cameraLook;

        public void Construct(CameraLook cameraLook)
        {
            _cameraLook = cameraLook;
        }

        private void Update()
        {
            aim.position = _cameraLook.HitPoint;
        }
    }
}