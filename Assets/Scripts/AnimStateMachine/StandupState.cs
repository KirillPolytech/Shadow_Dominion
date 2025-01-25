using Shadow_Dominion.StateMachine;
using UnityEngine;

public class AnimationStandUpState : IState
{
    private readonly Animator _animator;
    private readonly int _isStandUp;
    
    public AnimationStandUpState(Animator animator, int isStandUp)
    {
        _animator = animator;
        _isStandUp = isStandUp;
    }
    
    public override void Enter()
    {
        _animator.SetTrigger(_isStandUp);
    }

    public override void Exit()
    {
        
    }
}