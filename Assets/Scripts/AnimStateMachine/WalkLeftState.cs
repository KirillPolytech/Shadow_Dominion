using HellBeavers.Level.StateMachine;
using UnityEngine;

public class WalkLeftState : IState
{
    private static readonly int IsWalkLeft = Animator.StringToHash("IsWalkLeft");
    private readonly Animator _animator;

    public WalkLeftState(Animator animator)
    {
        _animator = animator;
    }
    
    public override void Enter()
    {
        _animator.SetBool(IsWalkLeft, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsWalkLeft, false);
    }
}