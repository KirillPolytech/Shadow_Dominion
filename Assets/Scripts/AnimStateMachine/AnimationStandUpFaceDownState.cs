using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationStandUpFaceDownState : IState
    {
        private readonly Animator _animator;
        private readonly int _hashcode;

        public AnimationStandUpFaceDownState(Animator animator, int hashcode)
        {
            _animator = animator;
            _hashcode = hashcode;
        }

        public override void Enter()
        {
            _animator.SetTrigger(_hashcode);
        }

        public override void Exit()
        {
        }
    }
}