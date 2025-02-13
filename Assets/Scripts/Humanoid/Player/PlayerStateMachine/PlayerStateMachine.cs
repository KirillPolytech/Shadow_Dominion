using System.Linq;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PlayerStateMachine : IStateMachine
    {
        public PlayerStateMachine(
            Main.Player player,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation,
            RigBuilder rootRig,
            BoneController[] boneController,
            CoroutineExecuter coroutineExecuter)
        {
            StandUpFaceUpState standUpFaceUpState =
                new StandUpFaceUpState(player, ragdollRoot, rootRig, playerAnimation, cameraLook, coroutineExecuter, this);
            StandUpFaceDownState standUpFaceDownState =
                new StandUpFaceDownState(player, ragdollRoot, rootRig, playerAnimation, cameraLook, coroutineExecuter, this);
            RagdollState ragdollState = new RagdollState(playerAnimation, cameraLook, rootRig, boneController);

            _states.Add(standUpFaceUpState);
            _states.Add(standUpFaceDownState);
            _states.Add(ragdollState);
            _states.Add(new DefaultState(playerAnimation));
            
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

            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
    }
}