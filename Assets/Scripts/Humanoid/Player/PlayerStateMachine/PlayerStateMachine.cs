using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine, IInitializable
    {
        private readonly Dictionary<IState, Dictionary<IState, bool>> _transitions = new();

        public PlayerStateMachine(
            Main.Player player,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneController)
        {
            IdleState idleState = new IdleState(playerAnimation);
            StandUpFaceUpState standUpFaceUpState =
                new StandUpFaceUpState(player, ragdollRoot, rootRig, playerAnimation, cameraLook);
            StandUpFaceDownState standUpFaceDownState =
                new StandUpFaceDownState(player, ragdollRoot, rootRig, playerAnimation, cameraLook);
            RagdollState ragdollState = new RagdollState(playerAnimation, cameraLook, rootRig, boneController);

            RunForwardState runForwardState = new RunForwardState(playerAnimation);
            RunBackwardState runBackwardState = new RunBackwardState(playerAnimation);

            WalkForwardState walkForwardState = new WalkForwardState(playerAnimation);
            WalkBackwardState walkBackwardState = new WalkBackwardState(playerAnimation);

            WalkLeftState walkLeftState = new WalkLeftState(playerAnimation);
            WalkRightState walkRightState = new WalkRightState(playerAnimation);
            
            WalkDiagonallyLeftState walkDiagonallyLeftState = new WalkDiagonallyLeftState(playerAnimation);
            WalkDiagonallyRightState walkDiagonallyRightState = new WalkDiagonallyRightState(playerAnimation);

            _transitions[idleState] = new Dictionary<IState, bool>
            {
                [standUpFaceUpState] = true,
                [walkForwardState] = true,
                [walkBackwardState] = true,
                [runForwardState] = true,
                [runBackwardState] = true,
                [walkLeftState] = true,
                [walkRightState] = true,
                [walkDiagonallyLeftState] = true,
                [walkDiagonallyRightState] = true,
            };

            _transitions[standUpFaceUpState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
            };

            _transitions[standUpFaceDownState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
            };

            _transitions[ragdollState] = new Dictionary<IState, bool>
            {
                [standUpFaceUpState] = true,
                [standUpFaceDownState] = true,
            };

            _transitions[runForwardState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkForwardState] = true,
                [walkBackwardState] = true,
                [runBackwardState] = true,
                [walkLeftState] = true,
                [walkRightState] = true,
                [ragdollState] = true,
            };

            _transitions[runBackwardState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkBackwardState] = true,
                [runForwardState] = true,
                [walkLeftState] = true,
                [walkRightState] = true,
                [ragdollState] = true
            };

            _transitions[walkForwardState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkLeftState] = true,
                [walkRightState] = true,
                [runForwardState] = true,
                [walkDiagonallyLeftState] = true,
                [walkDiagonallyRightState] = true,
            };

            _transitions[walkBackwardState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkLeftState] = true,
                [walkRightState] = true,
                [runBackwardState] = true,
                [walkDiagonallyLeftState] = true,
                [walkDiagonallyRightState] = true,
            };

            _transitions[walkLeftState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkRightState] = true,
                [walkForwardState] = true,
                [walkBackwardState] = true,
                [runForwardState] = true,
                [runBackwardState] = true,
                [walkDiagonallyLeftState] = true,
                [walkDiagonallyRightState] = true,
            };

            _transitions[walkRightState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                [walkLeftState] = true,
                [walkForwardState] = true,
                [walkBackwardState] = true,
                [runForwardState] = true,
                [runBackwardState] = true,
                [walkDiagonallyLeftState] = true,
                [walkDiagonallyRightState] = true,
            };
            
            _transitions[walkDiagonallyLeftState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                
                [walkLeftState] = true,
                [walkRightState] = true,
                
                [walkForwardState] = true,
                [walkBackwardState] = true,
                
                [runForwardState] = true,
                [runBackwardState] = true,
                [walkDiagonallyRightState] = true,
            };
            
            _transitions[walkDiagonallyRightState] = new Dictionary<IState, bool>
            {
                [idleState] = true,
                
                [walkLeftState] = true,
                [walkRightState] = true,
                
                [walkForwardState] = true,
                [walkBackwardState] = true,
                
                [runForwardState] = true,
                [runBackwardState] = true,
                
                [walkDiagonallyLeftState] = true,
            };
        }

        public void Initialize()
        {
            SetState<IdleState>();
        }

        public override void SetState<T>()
        {
            var state =
                _transitions.FirstOrDefault(x => x.Key.GetType() == typeof(T));
            
            if (state.Key == null)
                Debug.LogWarning($"No transition: CurrentState: {CurrentState.GetType()} NewState: {typeof(T)}");

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