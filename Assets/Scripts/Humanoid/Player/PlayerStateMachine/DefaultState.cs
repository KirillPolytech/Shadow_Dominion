using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;

namespace Shadow_Dominion.Player.StateMachine
{
    public class DefaultState : PlayerState
    {
        private readonly IInputHandler _inputHandler;
        private readonly PlayerMovement _playerMovement;
        
        public DefaultState(PlayerAnimation playerAnimation,
            PlayerMovement playerMovement,
            IInputHandler inputHandler) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _inputHandler = inputHandler;
        }

        public override void Enter()
        {
            _inputHandler.OnInputUpdate += _playerMovement.HandleInput;
        }

        public override void Exit()
        {
            _inputHandler.OnInputUpdate -= _playerMovement.HandleInput;
        }
    }
}