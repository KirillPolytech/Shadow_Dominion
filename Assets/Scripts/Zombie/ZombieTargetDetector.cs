using System;
using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class ZombieTargetDetector : MonoBehaviour
    {
        public event Action<IZombieTarget> OnDetectTarget;

        private void OnTriggerStay(Collider other)
        {
            IZombieTarget zombieTarget = other.gameObject.GetComponent<IZombieTarget>();
            if (zombieTarget == null)
                return;

            OnDetectTarget?.Invoke(zombieTarget);
        }
    }
}