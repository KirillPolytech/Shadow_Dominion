using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class GameplayState : IState
    {
        private readonly CursorService _cursorService;
        private readonly WindowsController _windowsController;

        public GameplayState(WindowsController windowsController, CursorService cursorService)
        {
            _cursorService = cursorService;
            _windowsController = windowsController;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            _cursorService.SetState(CursorLockMode.Locked);
        }

        public override void Exit()
        {
        }
    }
}