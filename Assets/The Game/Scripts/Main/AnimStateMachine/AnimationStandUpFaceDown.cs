using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationStandUpFaceDown : IState
    {
        private readonly Animator _animator;
        private readonly int _hashcode;
        
        public AnimationStandUpFaceDown(Animator animator, int hashcode)
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
            _animator.ResetTrigger(_hashcode);
        }
    }
}