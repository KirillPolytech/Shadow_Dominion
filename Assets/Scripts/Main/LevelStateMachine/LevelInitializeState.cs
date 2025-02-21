using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class LevelInitializeState : IState
    {
        private readonly CursorService _cursorService;
        private readonly WindowsController _windowsController;
        private readonly MirrorPlayerSpawner _mirrorPlayerSpawner;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly MirrorLevel _mirrorLevel;
        private readonly InitializeStateUI _initializeStateUI;
        private readonly float _waitTime = 30;

        public LevelInitializeState(
            WindowsController windowsController, 
            CursorService cursorService,
            MirrorPlayerSpawner mirrorPlayerSpawner,
            CoroutineExecuter coroutineExecuter,
            MirrorLevel mirrorLevel,
            InitializeStateUI initializeStateUI)
        {
            _windowsController = windowsController;
            _cursorService = cursorService;
            _mirrorPlayerSpawner = mirrorPlayerSpawner;
            _coroutineExecuter = coroutineExecuter;
            _mirrorLevel = mirrorLevel;
            _initializeStateUI = initializeStateUI;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<InitializeWindow>();
            _cursorService.SetState(CursorLockMode.Locked);

            _mirrorLevel.OnAllPlayersLoaded += () =>
            {
                List<KeyValuePair<NetworkConnectionToClient, Main.Player>> players =
                    _mirrorPlayerSpawner.playerInstances.ToList();
                foreach (var player in players)
                {
                    player.Value.PlayerStateMachine.SetState<InActiveState>();
                }

                _coroutineExecuter.Execute(WaitForSeconds(players));
            };
        }

        private IEnumerator WaitForSeconds(List<KeyValuePair<NetworkConnectionToClient, Main.Player>> activePlayers)
        {
            float t = 0;
            while (t < _waitTime)
            {
                t += Time.fixedDeltaTime;
                _initializeStateUI.SetWaitText($"Match starts in {(int)t}");
                yield return new WaitForFixedUpdate();
            }
            
            foreach (var player in activePlayers)
            {
                player.Value.PlayerStateMachine.SetState<DefaultState>();
            }
        }

        public override void Exit()
        {
        }
    }
}