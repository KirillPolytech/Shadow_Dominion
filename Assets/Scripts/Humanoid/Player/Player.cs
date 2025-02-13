using System.Collections.Generic;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid, IZombieTarget
    {
        public PlayerStateMachine playerStateMachine;
        public IEnumerable<Transform> Position { get; set; }
        
        private IInputHandler _monoInputHandler;
        private PlayerMovement _playerMovement;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Position = new []{transform};
        }

        public void Construct(
            PlayerMovement playerMovement,
            IInputHandler monoInputHandler,
            PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
            _monoInputHandler = monoInputHandler;
            _playerMovement = playerMovement;
        }

        private void OnEnable()
        {
            _monoInputHandler.OnInputUpdate += HandleInput;
        }

        private void OnDestroy()
        {
            _monoInputHandler.OnInputUpdate -= HandleInput;
        }
        
        private void HandleInput(InputData inputData)
        {
            if (playerStateMachine.CurrentState == null ||
                playerStateMachine.CurrentState.GetType() == typeof(RagdollState) 
                || playerStateMachine.CurrentState.GetType() == typeof(StandUpFaceDownState)
                || playerStateMachine.CurrentState.GetType() == typeof(StandUpFaceDownState))
            {
                _playerMovement.StandUp(inputData);
                return;
            }          
            
            _playerMovement.HandleInput(inputData, isLocalPlayer);
        }

        public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _rigidbody.position = pos;
            _rigidbody.rotation = rot;
        }
    }
}