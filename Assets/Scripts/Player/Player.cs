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
    }
}