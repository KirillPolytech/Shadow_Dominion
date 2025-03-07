using UnityEngine;

namespace Shadow_Dominion
{
    public abstract class MonobehaviourInitializer : MonoBehaviour
    {
        public bool IsInitialized { get; protected set; }
    }
}