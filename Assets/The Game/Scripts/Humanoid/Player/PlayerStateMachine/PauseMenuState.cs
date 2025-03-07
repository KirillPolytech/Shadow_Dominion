using Shadow_Dominion.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PauseMenuState : IState
    {
        private readonly WindowsController _windowsController;
        
        public PauseMenuState(WindowsController windowsController)
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
    }
}