using UnityEngine;

namespace Shadow_Dominion
{
    public class AimTarget : MonobehaviourInitializer
    {
        [SerializeField] private Transform aim;

        private CameraLook _cameraLook;

        public void Construct(CameraLook cameraLook)
        {
            _cameraLook = cameraLook;
            
            IsInitialized = true;
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
                return;
            
            aim.position = _cameraLook.HitPoint;
        }
    }
}