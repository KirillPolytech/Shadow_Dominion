using Shadow_Dominion.StateMachine;
using UnityEngine;

public class AnimationRunBackwardState : IState
{
    private readonly Animator _animator;
    private readonly int _isRunBackward;

    public AnimationRunBackwardState(Animator animator, int isRunBackward)
    {
        _animator = animator;
        _isRunBackward = isRunBackward;
    }

    public override void Enter()
    {
        _animator.SetBool(_isRunBackward, true);
    }

    public override void Exit()
    {
        _animator.SetBool(_isRunBackward, false);
    }
}