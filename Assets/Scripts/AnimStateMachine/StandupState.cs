using HellBeavers.Level.StateMachine;
using UnityEngine;

public class StandupState : IState
{
    private static readonly int IsStandUp = Animator.StringToHash("IsStandUp");
    private readonly Animator _animator;
    
    public StandupState(Animator animator)
    {
        _animator = animator;
    }
    
    public override void Enter()
    {
        _animator.SetBool(IsStandUp, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsStandUp, false);
    }
}