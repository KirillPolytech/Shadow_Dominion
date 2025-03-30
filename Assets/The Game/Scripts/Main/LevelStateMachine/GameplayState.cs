using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class GameplayState : IState
    {
        private readonly WindowsController _windowsController;

        public GameplayState(WindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            CursorService.SetState(CursorLockMode.Locked);
            
            foreach (var player in Object.FindObjectsByType<Main.MirrorPlayer>(FindObjectsSortMode.None))
            {
                player.PlayerStateMachine.SetState<DefaultState>();
            }
        }

        public override void Exit()
        {

        }
    }
}