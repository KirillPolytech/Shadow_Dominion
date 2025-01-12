using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public void Construct(ZombieSettings zombieSettings)
    {
        navMeshAgent.speed = zombieSettings.speed;
        navMeshAgent.acceleration = zombieSettings.acceleration;
    }
    
    public void MoveTo(IZombieTarget iZombieTarget)
    {
        navMeshAgent.SetDestination(iZombieTarget.Position.position);
    }
}