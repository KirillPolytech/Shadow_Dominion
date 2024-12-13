using HellBeavers.Level.StateMachine;
using UnityEngine;

public class WalkForwardState : IState
{
    private static readonly int IsWalkForward = Animator.StringToHash("IsWalkForward");
    
    private readonly Animator _animator;
    
    public WalkForwardState(Animator animator)
    {
        _animator = animator;
    }
    
    public override void Enter()
    {
        _animator.SetBool(IsWalkForward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsWalkForward, false);
    }
}