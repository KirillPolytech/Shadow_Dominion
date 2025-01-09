using UnityEngine;

namespace HellBeavers.Player
{
    public class Player : MonoBehaviour
    {
        private MonoInputHandler _monoInputHandler;

        public void Construct(MonoInputHandler monoInputHandler)
        {
            _monoInputHandler = monoInputHandler;
        }
    }
}