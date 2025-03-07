using System.Collections;
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

        public override void Enter()
        {
            _windowsController.OpenWindow<InitializeWindow>();
            CursorService.SetState(CursorLockMode.Locked);

            OnAllPlayersLoaded();
        }

        private void OnAllPlayersLoaded()
        {
            _coroutineExecuter.Execute(WaitForSeconds());
        }

        private IEnumerator WaitForSeconds()
        {
            float t = 0;
            while (t < _levelSO.InitializeWaitTime)
            {
                t += Time.fixedDeltaTime;
                _initializeStateUI.SetWaitText($"Match starts in {_levelSO.InitializeWaitTime - (int)t}");
                yield return new WaitForFixedUpdate();
            }
            
            _levelStateMachine.SetState<GameplayState>();
        }

        public override void Exit()
        {
        }
    }
}