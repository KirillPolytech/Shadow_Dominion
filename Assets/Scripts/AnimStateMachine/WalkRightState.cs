using HellBeavers.Level.StateMachine;
using UnityEngine;

public class WalkRightState : IState
{
    private static readonly int IsWalkRight = Animator.StringToHash("IsWalkRight");
    private readonly Animator _animator;

    public WalkRightState(Animator animator)
    {
        _animator = animator;
    }
    
    public override void Enter()
    {
        _animator.SetBool(IsWalkRight, true);
    }

    public override void Exit()
    {
        _animator.SetBool(IsWalkRight, false);
    }
}