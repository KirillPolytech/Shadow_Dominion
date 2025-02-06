using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationLayingState : IState
    {
        private readonly Animator _animator;
        private readonly int _isLaying;

        public AnimationLayingState(Animator animator, int isLaying)
        {
            _animator = animator;
            _isLaying = isLaying;
        }

        public override void Enter()
        {
            _animator.SetTrigger(_isLaying);
        }

        public override void Exit()
        {
        }
    }
}