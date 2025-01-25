using Shadow_Dominion.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Shadow_Dominion.Zombie
{
    public class ZombieMovement : MonoBehaviour
    {
        private const float DistanceError = 0.1f;
        
        [SerializeField] private NavMeshAgent navMeshAgent;

        private AnimationStateMachine _animationStateMachine;
        private IZombieTarget _iZombieTarget;

        public void Construct(Animator animator, ZombieSettings zombieSettings)
        {
            navMeshAgent.speed = zombieSettings.speed;
            navMeshAgent.acceleration = zombieSettings.acceleration;

            _animationStateMachine = new AnimationStateMachine(animator);
        }

        private void FixedUpdate()
        {
            HandleAnimations();
            Moving();
        }

        private void HandleAnimations()
        {
            if (navMeshAgent.remainingDistance > DistanceError) 
                _animationStateMachine.SetState<AnimationWalkForwardState>();
            else
                _animationStateMachine.SetState<AnimationIdleState>();
        }

        public void MoveTo(IZombieTarget iZombieTarget)
        {
            _iZombieTarget = iZombieTarget;
        }

        private void Moving()
        {
            if (_iZombieTarget == null)
                return;
            
            Vector3 destination = _iZombieTarget.Position.position - 
                                  (transform.position - _iZombieTarget.Position.position).normalized;
            navMeshAgent.SetDestination(destination);
        }
    }
}