using Shadow_Dominion.StateMachine;
using UnityEngine;

public class AnimationWalkLeftState : IState
{
    private readonly Animator _animator;
    private readonly int _isWalkLeft;

    public AnimationWalkLeftState(Animator animator, int isWalkLeft)
    {
        _animator = animator;
        _isWalkLeft = isWalkLeft;
    }
    
    public override void Enter()
    {
        _animator.SetBool(_isWalkLeft, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isWalkLeft, false);
    }
}