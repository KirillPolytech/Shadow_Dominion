using Shadow_Dominion.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.Player.StateMachine
{
    public class PauseMenuState : IState
    {
        private readonly WindowsController _windowsController;
        private readonly CursorService _cursorService;
        
        public PauseMenuState(WindowsController windowsController, CursorService cursorService)
        {
            _windowsController = windowsController;
            _cursorService = cursorService;
        }
        
        public override void Enter()
        {
            _cursorService.SetState(CursorLockMode.Confined);
            _windowsController.OpenWindow<PauseWindow>();
        }

        public override void Exit()
        {
            _cursorService.SetState(CursorLockMode.Locked);
        }
    }
}