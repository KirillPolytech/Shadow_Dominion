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
        private readonly Ak47 _ak47;
        private readonly PlayerStateMachine _playerStateMachine;

        public DefaultState(PlayerAnimation playerAnimation,
            PlayerMovement playerMovement,
            IInputHandler inputHandler,
            WindowsController windowsController,
            BoneController[] boneControllers,
            RigBuilder rigBuilder,
            CameraLook cameraLook,
            Ak47 ak47,
            PlayerStateMachine playerStateMachine) : base(playerAnimation)
        {
            _playerMovement = playerMovement;
            _inputHandler = inputHandler;
            _windowsController = windowsController;
            _boneControllers = boneControllers;
            _rigBuilder = rigBuilder;
            _cameraLook = cameraLook;
            _ak47 = ak47;
            _playerStateMachine = playerStateMachine;
        }

        public override void Enter()
        {
            _ak47.SetParent(_ak47.InitialParent);

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
            _inputHandler.OnInputUpdate += HandleTABInput;
            _inputHandler.OnInputUpdate += HandleDeathKeyInput;
            _inputHandler.OnInputUpdate += _ak47.HandleInput;
        }

        private void HandleTABInput(InputData inputData)
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
        
        private void HandleDeathKeyInput(InputData inputData)
        {
            if (!inputData.F_Down)
                return;

            _playerStateMachine.SetState<DeathState>();
        }

        public override void Exit()
        {
            _inputHandler.OnInputUpdate -= _playerMovement.HandleInput;
            _inputHandler.OnInputUpdate -= _playerAnimation.HandleAimRig;
            _inputHandler.OnInputUpdate -= HandleTABInput;
            _inputHandler.OnInputUpdate -= HandleDeathKeyInput;
            _inputHandler.OnInputUpdate -= _ak47.HandleInput;

            _playerMovement.IsRunning = false;
        }

        public override bool CanExit() => true;
    }
}