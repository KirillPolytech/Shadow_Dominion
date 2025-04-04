using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion.Main;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.StateMachine
{
    public class MovementStateMachine : IStateMachine
    {
        protected new readonly List<MovementState> _states = new();
        
        public new MovementState CurrentState { get; protected set; }
        
        public MovementStateMachine(
            CameraLook cameraLook,
            Rigidbody charRigidbody,
            PlayerSettings playerSettings,
            PlayerAnimation playerAnimation,
            PlayerStateMachine playerStateMachine,
            PlayerMovement playerMovement)
        {
            _states.Add(new WalkState(
                cameraLook, charRigidbody, playerSettings, playerAnimation, playerStateMachine, playerMovement));
            _states.Add(new RunState(
                cameraLook, charRigidbody, playerSettings, playerAnimation, playerStateMachine, playerMovement));
            _states.Add(new JumpState(
                cameraLook, charRigidbody, playerSettings, playerAnimation, playerStateMachine, playerMovement));
        }

        public override void SetState<T>()
        {
            MovementState state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state || (CurrentState != null && !CurrentState.CanExit()))
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
            
            Debug.Log($"Current movement state: {CurrentState.GetType()}, Time: {Time.time}");
        }
    }
}