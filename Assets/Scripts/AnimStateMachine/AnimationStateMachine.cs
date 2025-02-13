using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationStateMachine
    {
        private readonly int VelocityX = Animator.StringToHash("VelocityX");
        private readonly int VelocityY = Animator.StringToHash("VelocityY");
        
        private readonly int StandUp_Face_Up = Animator.StringToHash("StandUp_Face_Up");
        private readonly int StandUp_Face_Down = Animator.StringToHash("StandUp_Face_Down");
        
        private readonly Animator _animator;

        public AnimationStateMachine(Animator animator)
        {
            _animator = animator;
        }

        public void SetXY(float x, float y)
        {
            _animator.SetFloat(VelocityX, x);
            _animator.SetFloat(VelocityY, y);
        }

        public void StandUpFaceUp()
        {
            _animator.SetTrigger(StandUp_Face_Up);
        }
        
        public void StandUpFaceDown()
        {
            _animator.SetTrigger(StandUp_Face_Down);
        }
    }
}