using System;
using System.Linq;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine
    {
        public Action<IState> OnStateChanged;
        
        public PlayerStateMachine(
            Main.Player player,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneController,
            CoroutineExecuter coroutineExecuter, 
            PlayerMovement playerMovement,
            IInputHandler inputHandler)
        {
            StandUpFaceUpState standUpFaceUpState =
                new StandUpFaceUpState(player, ragdollRoot, rootRig, playerAnimation, cameraLook, coroutineExecuter, this);
            StandUpFaceDownState standUpFaceDownState =
                new StandUpFaceDownState(player, ragdollRoot, rootRig, playerAnimation, cameraLook, coroutineExecuter, this);
            RagdollState ragdollState = 
                new RagdollState(playerAnimation, cameraLook, rootRig, boneController, inputHandler, ragdollRoot, this);

            _states.Add(standUpFaceUpState);
            _states.Add(standUpFaceDownState);
            _states.Add(ragdollState);
            _states.Add(new DefaultState(playerAnimation, playerMovement, inputHandler));
            
            SetState<DefaultState>();
        }

        public override void SetState<T>()
        {
            var state = _states.First(x => x.GetType() == typeof(T));

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            OnStateChanged?.Invoke(CurrentState);
            
            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
        
        public void SetState(string stateName)
        {
            var state = _states.First(x => x.GetType().ToString() == stateName);

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            OnStateChanged?.Invoke(CurrentState);
            
            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
    }
}