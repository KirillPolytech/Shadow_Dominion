using Shadow_Dominion.Level.StateMachine;
using UnityEngine;

public class WalkRightState : IState
{
    private readonly Animator _animator;
    private readonly int _isWalkRight;

    public WalkRightState(Animator animator, int isWalkRight)
    {
        _animator = animator;
        _isWalkRight = isWalkRight;
    }
    
    public override void Enter()
    {
        _animator.SetBool(_isWalkRight, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isWalkRight, false);
    }
}