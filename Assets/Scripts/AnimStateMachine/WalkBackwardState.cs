using HellBeavers.Level.StateMachine;
using UnityEngine;

public class WalkBackwardState : IState
{
    private static readonly int IsWalkBackward = Animator.StringToHash("IsWalkBackward");
    
    private readonly Animator _animator;
    
    public WalkBackwardState(Animator animator)
    {
        _animator = animator;
    }
    
    public override void Enter()
    {
        _animator.SetBool(IsWalkBackward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsWalkBackward, false);
    }
}