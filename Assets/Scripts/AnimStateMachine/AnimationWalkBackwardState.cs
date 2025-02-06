using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationWalkBackwardState : IState
    {
        private readonly Animator _animator;
        private readonly int _isWalkBackward;

        public AnimationWalkBackwardState(Animator animator, int isWalkBackward)
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
}