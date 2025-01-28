using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class AnimationStandUpFaceDownState : IState
    {
        private readonly Animator _animator;
        private readonly int _isStandUp;
    
        public AnimationStandUpFaceDownState(Animator animator, int isStandUp)
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
}