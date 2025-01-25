using System.Linq;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player
{
    public class AnimationStateMachine : IStateMachine
    {
        private readonly int IsIdle = Animator.StringToHash("IsIdle");
        private readonly int IsWalkForward = Animator.StringToHash("IsWalkForward");
        private readonly int IsWalkBackward = Animator.StringToHash("IsWalkBackward");
        private readonly int IsWalkLeft = Animator.StringToHash("IsWalkLeft");
        private readonly int IsWalkRight = Animator.StringToHash("IsWalkRight");
        private readonly int IsRunForward = Animator.StringToHash("IsRunForward");
        private readonly int IsRunBackward = Animator.StringToHash("IsRunBackward");
        private readonly int StandUpHashCode = Animator.StringToHash("StandUp");
        private readonly int Laying = Animator.StringToHash("Laying");

        public AnimationStateMachine(Animator animator)
        {
            _states.Add(new AnimationIdleState(animator, IsIdle));
            _states.Add(new AnimationWalkForwardState(animator, IsWalkForward));
            _states.Add(new AnimationWalkBackwardState(animator, IsWalkBackward));
            _states.Add(new AnimationWalkLeftState(animator, IsWalkLeft));
            _states.Add(new AnimationWalkRightState(animator, IsWalkRight));
            _states.Add(new AnimationRunForwardState(animator, IsRunForward));
            _states.Add(new AnimationRunBackwardState(animator, IsRunBackward));
            _states.Add(new AnimationStandUpState(animator, StandUpHashCode));
            _states.Add(new AnimationLayingState(animator, Laying));
        }

        public override void SetState<T>()
        {
            IState state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            Debug.Log($"Current anim state: {CurrentState.GetType()}");
        }
    }
}