using HellBeavers.Level.StateMachine;
using UnityEngine;

public class WalkBackwardState : IState
{
    private readonly Animator _animator;
    private readonly int _isWalkBackward;
    
    public WalkBackwardState(Animator animator, int isWalkBackward)
    {
        _animator = animator;
        _isWalkBackward = isWalkBackward;
    }
    
    public override void Enter()
    {
        _animator.SetBool(_isWalkBackward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isWalkBackward, false);
    }
}