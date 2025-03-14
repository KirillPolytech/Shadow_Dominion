using System.Collections;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class LevelInitializeState : IState
    {
        private readonly WindowsController _windowsController;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly InitializeStateUI _initializeStateUI;
        private readonly LevelStateMachine _levelStateMachine;
        private readonly LevelSO _levelSO;
        
        public LevelInitializeState(
            WindowsController windowsController, 
            CoroutineExecuter coroutineExecuter,
            InitializeStateUI initializeStateUI,
            LevelStateMachine levelStateMachine,
            LevelSO levelSo)
        {
            _windowsController = windowsController;
            _coroutineExecuter = coroutineExecuter;
            _initializeStateUI = initializeStateUI;
            _levelStateMachine = levelStateMachine;
            _levelSO = levelSo;
        }

        ~LevelInitializeState()
        {
            _coroutineExecuter.Stop(WaitForSeconds());
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<InitializeWindow>();
            CursorService.SetState(CursorLockMode.Locked);
            
            foreach (var player in Object.FindObjectsByType<Main.Player>(FindObjectsSortMode.None))
            {
                player.PlayerStateMachine.SetState<InActiveState>();
            }

            _coroutineExecuter.Execute(WaitForSeconds());
        }

        private IEnumerator WaitForSeconds()
        {
            float t = 0;
            while (t < _levelSO.InitializeWaitTime || MirrorServer.Instance.SpawnedPlayerInstances.Count < MirrorServer.Instance.Connections.Count)
            {
                t += Time.fixedDeltaTime;
                _initializeStateUI.SetWaitText($"Match starts in {_levelSO.InitializeWaitTime - (int)t}");
                yield return new WaitForFixedUpdate();
            }
            
            SpawnPointSyncer.Instance.Reset();
            
            _levelStateMachine.SetState<GameplayState>();
        }

        public override void Exit()
        {
        }
    }
}