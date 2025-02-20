using System.Collections.Generic;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class LevelInitializeState : IState
    {
        private readonly CursorService _cursorService;
        private readonly WindowsController _windowsController;
        private readonly PlayerPool _playerPool;

        public LevelInitializeState(
            WindowsController windowsController, 
            CursorService cursorService,
            PlayerPool playerPool)
        {
            _windowsController = windowsController;
            _cursorService = cursorService;
            _playerPool = playerPool;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<MainWindow>();
            _cursorService.SetState(CursorLockMode.Locked);
            
            IEnumerable<Main.Player> activePlayers = _playerPool.GetActivePlayers();
            foreach (var player in activePlayers)
            {
                player.PlayerStateMachine.SetState<InActiveState>();
            }
        }

        public override void Exit()
        {
        }
    }
}