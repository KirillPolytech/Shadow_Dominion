using Shadow_Dominion.Level.StateMachine;
using UnityEngine;

public class IdleState : IState
{
    private readonly Animator _animator;
    private readonly int _isIdle;

    public IdleState(Animator animator, int isIdle)
    {
        _animator = animator;
        _isIdle = isIdle;
    }

    public override void Enter()
    {
        _animator.SetBool(_isIdle, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isIdle, false);
    }
}