using HellBeavers.Level.StateMachine;
using UnityEngine;

public class IdleState : IState
{
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");
    private readonly Animator _animator;

    public IdleState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool(IsIdle, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsIdle, false);
    }
}