using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine
    {
        private readonly Dictionary<IState, Dictionary<IState, bool>> _transitions = new();

        private readonly IdleState _idleState;
        private readonly StandUpState _standUpState;
        private readonly RagdollState _ragdollState;
        private readonly RunForwardState _runForwardState;
        private readonly RunBackwardState _runBackwardState;

        public PlayerStateMachine(
            PlayerMovement playerMovement,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneController)
        {
            _idleState = new IdleState(playerMovement, playerAnimation);
            _standUpState = new StandUpState(playerMovement, playerAnimation);
            _ragdollState = new RagdollState(playerMovement, playerAnimation, rootRig, boneController);
            _runForwardState = new RunForwardState();
            _runBackwardState = new RunBackwardState();

            _transitions[_idleState] = new Dictionary<IState, bool>
            {
                [_standUpState] = true,
                [_runForwardState] = true,
                [_runBackwardState] = true,
            };
            
            _transitions[_standUpState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
            };

            _transitions[_ragdollState] = new Dictionary<IState, bool>
            {
                [_standUpState] = true,
            };
            
            _transitions[_runForwardState] = new Dictionary<IState, bool>
            {
                [_ragdollState] = true
            };
            
            _transitions[_runBackwardState] = new Dictionary<IState, bool>
            {
                [_runForwardState] = true,
                [_ragdollState] = true
            };
        }

        public override void SetState<T>()
        {
            var state =
                _transitions.First(x => x.Key.GetType() == typeof(T));

            if (CurrentState == state.Key)
                return;
            
            if (CurrentState != null)
                if (!_transitions[CurrentState][state.Key])
                    return;
            
            CurrentState?.Exit();
            CurrentState = state.Key;
            CurrentState.Enter();
        }
    }
}