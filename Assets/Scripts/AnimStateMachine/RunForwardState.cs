using HellBeavers.Level.StateMachine;
using UnityEngine;

public class RunForwardState : IState
{
    private static readonly int IsRunForward = Animator.StringToHash("IsRunForward");
    private readonly Animator _animator;

    public RunForwardState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool(IsRunForward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsRunForward, false);
    }
}