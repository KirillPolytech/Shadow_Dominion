using System.Linq;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.AnimStateMachine
{
    public class AnimationStateMachine : IStateMachine
    {
        private readonly int VelocityX = Animator.StringToHash("VelocityX");
        private readonly int VelocityY = Animator.StringToHash("VelocityY");

        private readonly int StandUp_Face_Up = Animator.StringToHash("StandUp_Face_Up");
        private readonly int StandUp_Face_Down = Animator.StringToHash("StandUp_Face_Down");

        private readonly int Laying = Animator.StringToHash("Laying");
        
        private readonly int Idle = Animator.StringToHash("Idle");

        private readonly Animator _animator;

        public AnimationStateMachine(Animator animator)
        {
            _animator = animator;

            _states.Add(new AnimationStandUpFaceDown(animator, StandUp_Face_Down));
            _states.Add(new AnimationStandUpFaceUp(animator, StandUp_Face_Up));
            _states.Add(new AnimationLay(animator, Laying));
            _states.Add(new AnimationIdleState(animator, Idle));
        }

        public override void SetState<T>()
        {
            IState state = _states.First(x => x.GetType() == typeof(T));
            
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        public void SetXY(float x, float y)
        {
            _animator.SetFloat(VelocityX, x);
            _animator.SetFloat(VelocityY, y);
        }
    }
}