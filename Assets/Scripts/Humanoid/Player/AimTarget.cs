using UnityEngine;

namespace Shadow_Dominion
{
    public class AimTarget : MonoBehaviour, IInitialize
    {
        [SerializeField] private Transform aim;

        private CameraLook _cameraLook;

        public void Construct(CameraLook cameraLook)
        {
            _cameraLook = cameraLook;
            
            IInitialize.IsInitialized = true;
        }

        private void FixedUpdate()
        {
            if (!IInitialize.IsInitialized)
                return;
            
            aim.position = _cameraLook.HitPoint;
        }
    }
}