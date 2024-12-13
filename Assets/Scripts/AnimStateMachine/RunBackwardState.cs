using HellBeavers.Level.StateMachine;
using UnityEngine;

public class RunBackwardState : IState
{
    private static readonly int IsRunBackward = Animator.StringToHash("IsRunBackward");
    private readonly Animator _animator;

    public RunBackwardState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool(IsRunBackward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsRunBackward, false);
    }
}