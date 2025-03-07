using System.Collections.Generic;
using System.Linq;
using Mirror;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class GameplayState : IState
    {
        private readonly WindowsController _windowsController;

        public GameplayState(
            WindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            CursorService.SetState(CursorLockMode.Locked);

            var players = Object.FindObjectsByType<Main.Player>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                player.PlayerStateMachine.SetState<DefaultState>();
            }
        }

        public override void Exit()
        {
        }
    }
}