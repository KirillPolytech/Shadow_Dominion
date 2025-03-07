using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class DefaultState : PlayerState
    {
        private readonly IInputHandler _inputHandler;
        private readonly PlayerMovement _playerMovement;
        private readonly WindowsController _windowsController;

        public DefaultState(PlayerAnimation playerAnimation,
            PlayerMovement playerMovement,
            IInputHandler inputHandler,
            WindowsController windowsController) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _inputHandler = inputHandler;
            _windowsController = windowsController;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            
            _inputHandler.OnInputUpdate += _playerMovement.HandleInput;
            _inputHandler.OnInputUpdate += _playerAnimation.HandleAimRig;
            _inputHandler.OnInputUpdate += HandleInput;

        }

        private void HandleInput(InputData inputData)
        {
            if (!inputData.TAB)
                return;

            if (_windowsController.Current.GetType() == typeof(StatisticWindow))
            {
                _windowsController.OpenWindow<MainWindow>();
                return;
            }
            
            _windowsController.OpenWindow<StatisticWindow>();
        }

        public override void Exit()
        {
            _inputHandler.OnInputUpdate -= _playerMovement.HandleInput;
            _inputHandler.OnInputUpdate -= _playerAnimation.HandleAimRig;
            _inputHandler.OnInputUpdate -= HandleInput;
        }
    }
}