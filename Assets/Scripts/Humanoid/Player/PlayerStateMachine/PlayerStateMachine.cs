using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine, IInitializable
    {
        private readonly Dictionary<IState, Dictionary<IState, bool>> _transitions = new();

        private readonly IdleState _idleState;
        private readonly StandUpState _standUpState;
        private readonly RagdollState _ragdollState;

        private readonly WalkForwardState _walkForwardState;
        private readonly WalkBackwardState _walkBackwardState;

        private readonly RunForwardState _runForwardState;
        private readonly RunBackwardState _runBackwardState;

        private readonly WalkLeftState _walkLeftState;
        private readonly WalkRightState _walkRightState;

        public PlayerStateMachine(
            Main.Player player,
            PlayerMovement playerMovement,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneController)
        {
            _idleState = new IdleState(playerAnimation);
            _standUpState = new StandUpState(player, ragdollRoot, playerMovement, rootRig, playerAnimation, cameraLook);
            _ragdollState = new RagdollState(playerMovement, playerAnimation, cameraLook, rootRig, boneController);

            _runForwardState = new RunForwardState(playerAnimation);
            _runBackwardState = new RunBackwardState(playerAnimation);

            _walkForwardState = new WalkForwardState(playerAnimation);
            _walkBackwardState = new WalkBackwardState(playerAnimation);

            _walkLeftState = new WalkLeftState(playerAnimation);
            _walkRightState = new WalkRightState(playerAnimation);

            _transitions[_idleState] = new Dictionary<IState, bool>
            {
                [_standUpState] = true,
                [_walkForwardState] = true,
                [_walkBackwardState] = true,
                [_runForwardState] = true,
                [_runBackwardState] = true,
                [_walkLeftState] = true,
                [_walkRightState] = true,
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
                [_idleState] = true,
                [_walkForwardState] = true,
                [_walkBackwardState] = true,
                [_runBackwardState] = true,
                [_walkLeftState] = true,
                [_walkRightState] = true,
                [_ragdollState] = true,
            };

            _transitions[_runBackwardState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
                [_walkBackwardState] = true,
                [_runForwardState] = true,
                [_walkLeftState] = true,
                [_walkRightState] = true,
                [_ragdollState] = true
            };

            _transitions[_walkForwardState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
                [_walkLeftState] = true,
                [_walkRightState] = true,
                [_runForwardState] = true,
            };

            _transitions[_walkBackwardState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
                [_walkLeftState] = true,
                [_walkRightState] = true,
                [_runBackwardState] = true,
            };

            _transitions[_walkLeftState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
                [_walkRightState] = true,
                [_walkForwardState] = true,
                [_walkBackwardState] = true,
                [_runForwardState] = true,
                [_runBackwardState] = true,
            };

            _transitions[_walkRightState] = new Dictionary<IState, bool>
            {
                [_idleState] = true,
                [_walkLeftState] = true,
                [_walkForwardState] = true,
                [_walkBackwardState] = true,
                [_runForwardState] = true,
                [_runBackwardState] = true,
            };
        }

        public void Initialize()
        {
            SetState<IdleState>();
        }

        public override void SetState<T>()
        {
            var state =
                _transitions.First(x => x.Key.GetType() == typeof(T));

            if (CurrentState == state.Key)
                return;

            if (CurrentState != null)
                if (!_transitions[CurrentState].TryGetValue(state.Key, out bool value))
                    if (!value)
                        return;

            CurrentState?.Exit();
            CurrentState = state.Key;
            CurrentState.Enter();

            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
    }
}