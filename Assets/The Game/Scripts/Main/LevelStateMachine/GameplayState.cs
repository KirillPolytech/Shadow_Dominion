using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class GameplayState : IState
    {
        private readonly WindowsController _windowsController;
        private int _deadPlayers;

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
                player.OnDead += OnDeath;
                
                player.PlayerStateMachine.SetState<DefaultState>();
            }
        }

        private void OnDeath()
        {
            if (++_deadPlayers < MirrorServer.Instance.SpawnedPlayerInstances.Count - 1)
                return;
            
            NetworkClient.Send(new LevelState(typeof(LevelInitializeState).ToString()));

            _deadPlayers = 0;
        }

        public override void Exit()
        {
            foreach (var player in Object.FindObjectsByType<Main.MirrorPlayer>(FindObjectsSortMode.None))
            {
                player.OnDead -= OnDeath;
            }
        }
    }
}