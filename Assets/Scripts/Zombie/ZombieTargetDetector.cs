using System;
using UnityEngine;

public class ZombieTargetDetector : MonoBehaviour
{
    public event Action<IZombieTarget> OnDetectTarget;

    private void OnTriggerEnter(Collider other)
    {
        IZombieTarget zombieTarget = other.gameObject.GetComponent<IZombieTarget>();
        if (zombieTarget == null)
            return;

        OnDetectTarget?.Invoke(zombieTarget);
    }
}