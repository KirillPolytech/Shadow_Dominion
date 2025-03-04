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
        private readonly InitializeStateUI _initializeStateUI;
        private readonly LevelStateMachine _levelStateMachine;
        private readonly LevelSO _levelSO;
        
        public LevelInitializeState(
            WindowsController windowsController, 
            CursorService cursorService,
            MirrorPlayerSpawner mirrorPlayerSpawner,
            CoroutineExecuter coroutineExecuter,
            InitializeStateUI initializeStateUI,
            LevelStateMachine levelStateMachine,
            LevelSO levelSo)
        {
            _windowsController = windowsController;
            _cursorService = cursorService;
            _mirrorPlayerSpawner = mirrorPlayerSpawner;
            _coroutineExecuter = coroutineExecuter;
            _initializeStateUI = initializeStateUI;
            _levelStateMachine = levelStateMachine;
            _levelSO = levelSo;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<InitializeWindow>();
            _cursorService.SetState(CursorLockMode.Locked);

            MirrorLevel.Instance.OnAllPlayersLoaded += OnAllPlayersLoaded;
        }

        private void OnAllPlayersLoaded()
        {
            List<KeyValuePair<NetworkConnectionToClient, Main.Player>> players =
                _mirrorPlayerSpawner.playerInstances.ToList();
            
            foreach (var player in players)
            {
                player.Value.PlayerStateMachine.SetState<InActiveState>();
            }

            _coroutineExecuter.Execute(WaitForSeconds());
        }

        private IEnumerator WaitForSeconds()
        {
            float t = 0;
            while (t < _levelSO.InitializeWaitTime)
            {
                t += Time.fixedDeltaTime;
                _initializeStateUI.SetWaitText($"Match starts in {(int)t}");
                yield return new WaitForFixedUpdate();
            }
            
            _levelStateMachine.SetState<GameplayState>();
        }

        public override void Exit()
        {
            MirrorLevel.Instance.OnAllPlayersLoaded -= OnAllPlayersLoaded;
        }
    }
}