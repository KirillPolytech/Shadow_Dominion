using System.Linq;
using Shadow_Dominion.Level.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
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
        private readonly int IsStandUpHashCode = Animator.StringToHash("IsStandUp");
        private readonly int StandUpHashCode = Animator.StringToHash("StandUp");

        public AnimationStateMachine(Animator animator)
        {
            _states.Add(new IdleState(animator, IsIdle));
            _states.Add(new WalkForwardState(animator, IsWalkForward));
            _states.Add(new WalkBackwardState(animator, IsWalkBackward));
            _states.Add(new WalkLeftState(animator, IsWalkLeft));
            _states.Add(new WalkRightState(animator, IsWalkRight));
            _states.Add(new RunForwardState(animator, IsRunForward));
            _states.Add(new RunBackwardState(animator, IsRunBackward));
            _states.Add(new StandupState(animator, StandUpHashCode));
        }

        public override void SetState<T>()
        {
            IState state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            Debug.Log($"Current state: {CurrentState.GetType()}");
        }
    }
}