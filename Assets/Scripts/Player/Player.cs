using UnityEngine;

namespace HellBeavers.Player
{
    public class Player : MonoBehaviour
    {
        private MonoInputHandler _monoInputHandler;
        private CopyMotion _copyMotion;

        public void Construct(MonoInputHandler monoInputHandler, CopyMotion copyMotion)
        {
            _monoInputHandler = monoInputHandler;
            _copyMotion = copyMotion;
        }

        private void OnEnable()
        {
            _monoInputHandler.OnInputUpdate += HandleRagdoll;
        }

        private void HandleRagdoll(InputData inputData)
        {
            _copyMotion.IsCopyPos(!inputData.E);
            _copyMotion.IsCopyRot(!inputData.E);
            _copyMotion.SmoothDeactivate(inputData.E);
        }

        private void OnDisable()
        {
            _monoInputHandler.OnInputUpdate -= HandleRagdoll;
        }
    }
}