using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PauseMenuState : PlayerState
    {
        private readonly WindowsController _windowsController;
        private readonly PlayerStateMachine _playerStateMachine;
        
        public PauseMenuState(
            WindowsController windowsController, 
            PlayerAnimation playerAnimation, 
            PlayerStateMachine playerStateMachine) : base(playerAnimation)
        {
            _windowsController = windowsController;
            _playerStateMachine = playerStateMachine;
        }
        
        public override void Enter()
        {
            CursorService.SetState(CursorLockMode.Confined);
            _windowsController.OpenWindow<PauseWindow>();
            
            //_playerStateMachine.SetState<Idle>();
        }

        public override void Exit()
        {
            CursorService.SetState(CursorLockMode.Locked);
        }

        public override bool CanExit() => true;
    }
}