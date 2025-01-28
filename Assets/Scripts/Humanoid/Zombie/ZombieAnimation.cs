using Shadow_Dominion.Player;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    private const float DistanceError = 0.1f;

    private AnimationStateMachine _animationStateMachine;
    private NavMeshAgent _navMeshAgent;

    public void Construct(Animator animator, NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;

        _animationStateMachine = new AnimationStateMachine(animator);
    }
    
    private void FixedUpdate()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (_navMeshAgent.remainingDistance > DistanceError) 
            _animationStateMachine.SetState<AnimationWalkForwardState>();
        else
            _animationStateMachine.SetState<AnimationIdleState>();
    }
}
