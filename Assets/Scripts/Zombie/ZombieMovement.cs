using UnityEngine;
using UnityEngine.AI;

namespace Shadow_Dominion.Zombie
{
    public class ZombieMovement : MonoBehaviour
    {
        private const float DistanceError = 0.1f;
        
        [SerializeField] private NavMeshAgent navMeshAgent;

        private AnimationStateMachine _animationStateMachine;

        public void Construct(Animator animator, ZombieSettings zombieSettings)
        {
            navMeshAgent.speed = zombieSettings.speed;
            navMeshAgent.acceleration = zombieSettings.acceleration;

            _animationStateMachine = new AnimationStateMachine(animator);
        }

        private void FixedUpdate()
        {
            HandleAnimations();
        }

        private void HandleAnimations()
        {
            if (navMeshAgent.remainingDistance > DistanceError) 
                _animationStateMachine.SetState<WalkForwardState>();
            else
                _animationStateMachine.SetState<IdleState>();
        }

        public void MoveTo(IZombieTarget iZombieTarget)
        {
            navMeshAgent.SetDestination(iZombieTarget.Position.position);
        }
    }
}