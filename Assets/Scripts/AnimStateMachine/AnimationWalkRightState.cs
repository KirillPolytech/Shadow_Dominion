using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationWalkRightState : IState
    {
        private readonly Animator _animator;
        private readonly int _isWalkRight;

        public AnimationWalkRightState(Animator animator, int isWalkRight)
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
}