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
        private readonly CursorService _cursorService;
        private readonly WindowsController _windowsController;

        public GameplayState(
            WindowsController windowsController, 
            CursorService cursorService)
        {
            _cursorService = cursorService;
            _windowsController = windowsController;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            _cursorService.SetState(CursorLockMode.Locked);
            
            List<KeyValuePair<NetworkConnectionToClient, Main.Player>> players = MirrorPlayerSpawner.Instance.playerInstances.ToList();
            
            foreach (var player in players)
            {
                player.Value.PlayerStateMachine.SetState<DefaultState>();
            }
        }

        public override void Exit()
        {
        }
    }
}