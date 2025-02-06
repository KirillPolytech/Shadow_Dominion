using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationWalkDiagonallyLeftState : IState
    {
        private readonly Animator _animator;
        private readonly int _hashcode;

        public AnimationWalkDiagonallyLeftState(Animator animator, int hashcode)
        {
            _animator = animator;
            _hashcode = hashcode;
        }

        public override void Enter()
        {
            _animator.SetBool(_hashcode, true);
        }

        public override void Exit()
        {
            _animator.SetBool(_hashcode, false);
        }
    }
}