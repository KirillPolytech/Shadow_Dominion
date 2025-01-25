using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class PauseState : IState
    {
        private readonly WindowsController _windowsController;
        
        public PauseState(WindowsController windowsController)
        {
            _windowsController = windowsController;
        }
        
        public override void Enter()
        {
            _windowsController.OpenWindow<PauseWindow>();
        }

        public override void Exit()
        {
            
        }
    }
}