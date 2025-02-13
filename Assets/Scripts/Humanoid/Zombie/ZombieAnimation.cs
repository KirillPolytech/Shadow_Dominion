using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    private const float DistanceError = 0.1f;

    private NavMeshAgent _navMeshAgent;

    public void Construct(Animator animator, NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
}
