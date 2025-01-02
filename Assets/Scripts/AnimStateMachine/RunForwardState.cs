using HellBeavers.Level.StateMachine;
using UnityEngine;

public class RunForwardState : IState
{
    private readonly Animator _animator;
    private readonly int _isRunForward;
    
    public RunForwardState(Animator animator, int isRunForward)
    {
        _animator = animator;
        _isRunForward = isRunForward;
    }

    public override void Enter()
    {
        _animator.SetBool(_isRunForward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isRunForward, false);
    }
}