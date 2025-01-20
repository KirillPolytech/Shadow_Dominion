using Shadow_Dominion.Level.StateMachine;
using UnityEngine;

public class LayingState : IState
{
    private readonly Animator _animator;
    private readonly int _isLaying;
    
    public LayingState(Animator animator, int isLaying)
    {
        _animator = animator;
        _isLaying = isLaying;
    }
    
    public override void Enter()
    {
        _animator.SetTrigger(_isLaying);
    }

    public override void Exit()
    {
    }
}
