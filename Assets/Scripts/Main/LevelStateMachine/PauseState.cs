using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class PauseState : IState
    {
        private readonly WindowsController _windowsController;
        private readonly CursorService _cursorService;
        
        public PauseState(WindowsController windowsController, CursorService cursorService)
        {
            _windowsController = windowsController;
            _cursorService = cursorService;
        }
        
        public override void Enter()
        {
            _windowsController.OpenWindow<PauseWindow>();
            _cursorService.SetState(CursorLockMode.Confined);
        }

        public override void Exit()
        {
            
        }
    }
}