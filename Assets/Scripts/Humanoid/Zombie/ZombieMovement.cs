using UnityEngine;
using UnityEngine.AI;

namespace Shadow_Dominion.Zombie
{
    public class ZombieMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private IZombieTarget _iZombieTarget;

        public void Construct(NavMeshAgent navMeshAgent, ZombieSettings zombieSettings)
        {
            navMeshAgent.speed = zombieSettings.speed;
            navMeshAgent.acceleration = zombieSettings.acceleration;
        }

        private void FixedUpdate()
        {
            Moving();
        }

        public void MoveToTarget(IZombieTarget iZombieTarget)
        {
            _iZombieTarget = iZombieTarget;
        }

        private void Moving()
        {
            if (_iZombieTarget == null)
                return;

            var enumerator = _iZombieTarget.PlayersTrasform;
            Vector3 destination = enumerator.position - (transform.position - enumerator.position).normalized;
            _navMeshAgent.SetDestination(destination);
        }
    }
}