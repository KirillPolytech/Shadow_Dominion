using UnityEngine;

namespace Shadow_Dominion
{
    public class AimTarget : MonoBehaviour
    {
        [SerializeField] private Transform aim;

        private CameraLook _cameraLook;

        public void Construct(CameraLook cameraLook)
        {
            _cameraLook = cameraLook;
        }

        private void FixedUpdate()
        {
            aim.position = _cameraLook.HitPoint;
        }
    }
}