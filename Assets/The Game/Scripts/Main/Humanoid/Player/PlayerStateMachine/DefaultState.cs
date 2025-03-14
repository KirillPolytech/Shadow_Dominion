using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Main;
using UnityEngine.Animations.Rigging;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class DefaultState : PlayerState
    {
        private readonly IInputHandler _inputHandler;
        private readonly PlayerMovement _playerMovement;
        private readonly WindowsController _windowsController;
        private readonly BoneController[] _boneControllers;
        private readonly RigBuilder _rigBuilder;
        private readonly CameraLook _cameraLook;

        public DefaultState(PlayerAnimation playerAnimation,
            PlayerMovement playerMovement,
            IInputHandler inputHandler,
            WindowsController windowsController,
            BoneController[] boneControllers,
            RigBuilder rigBuilder,
            CameraLook cameraLook) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _inputHandler = inputHandler;
            _windowsController = windowsController;
            _boneControllers = boneControllers;
            _rigBuilder = rigBuilder;
            _cameraLook = cameraLook;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();
            
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
            
            foreach (var boneController in _boneControllers)
            {
                boneController.IsPositionApplying(true);
                boneController.IsRotationApplying(true);
            }
            
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

        public override bool CanExit() => true;
    }
}