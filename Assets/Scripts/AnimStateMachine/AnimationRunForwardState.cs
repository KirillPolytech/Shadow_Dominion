using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationRunForwardState : IState
    {
        private readonly Animator _animator;
        private readonly int _isRunForward;

        public AnimationRunForwardState(Animator animator, int isRunForward)
        {
            _animator = animator;
            _isRunForward = isRunForward;
        }

        public override void Enter()
        {
            _animator.SetBool(_isRunForward, true);
        }

        public override void Exit()
        {
            _animator.SetBool(_isRunForward, false);
        }
    }
}