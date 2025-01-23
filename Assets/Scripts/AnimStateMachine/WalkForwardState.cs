using Shadow_Dominion.StateMachine;
using UnityEngine;

public class WalkForwardState : IState
{
    private readonly Animator _animator;
    private readonly int _isWalkForward;
    
    public WalkForwardState(Animator animator, int isWalkForward)
    {
        _animator = animator;
        _isWalkForward = isWalkForward;
    }
    
    public override void Enter()
    {
        _animator.SetBool(_isWalkForward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isWalkForward, false);
    }
}