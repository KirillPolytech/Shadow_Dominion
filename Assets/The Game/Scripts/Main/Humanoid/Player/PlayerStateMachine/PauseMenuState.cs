using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PauseMenuState : PlayerState
    {
        private readonly WindowsController _windowsController;
        
        public PauseMenuState(WindowsController windowsController, PlayerAnimation playerAnimation) : base(playerAnimation)
        {
            _windowsController = windowsController;
        }
        
        public override void Enter()
        {
            CursorService.SetState(CursorLockMode.Confined);
            _windowsController.OpenWindow<PauseWindow>();
        }

        public override void Exit()
        {
            CursorService.SetState(CursorLockMode.Locked);
        }

        public override bool CanExit() => true;
    }
}