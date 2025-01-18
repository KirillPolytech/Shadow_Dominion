using System;
using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class ZombieTargetDetector : MonoBehaviour
    {
        public event Action<IZombieTarget> OnDetectTarget;

        private void OnTriggerStay(Collider other)
        {
            other.TryGetComponent(out IZombieTarget zombieTarget);
            if (zombieTarget == null)
                return;

            OnDetectTarget?.Invoke(zombieTarget);
        }
    }
}