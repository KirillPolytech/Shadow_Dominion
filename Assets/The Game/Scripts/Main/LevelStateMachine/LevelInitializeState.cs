using Mirror;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class LevelInitializeState : IState
    {
        private readonly WindowsController _windowsController;
        private readonly InitializeStateUI _initializeStateUI;
        private readonly LevelStateMachine _levelStateMachine;
        private readonly LevelSO _levelSO;

        private float _currentTime;

        public LevelInitializeState(
            WindowsController windowsController,
            InitializeStateUI initializeStateUI,
            LevelStateMachine levelStateMachine,
            LevelSO levelSo)
        {
            _windowsController = windowsController;
            _initializeStateUI = initializeStateUI;
            _levelStateMachine = levelStateMachine;
            _levelSO = levelSo;
        }

        ~LevelInitializeState()
        {
            _levelStateMachine.OnUpdate -= WaitForSeconds;
        }

        public override void Enter()
        {
            _windowsController.OpenWindow<InitializeWindow>();
            CursorService.SetState(CursorLockMode.Locked);

            foreach (var player in Object.FindObjectsByType<Main.MirrorPlayer>(FindObjectsSortMode.None))
            {
                player.PlayerStateMachine.SetState<InActiveState>();
            }

            _currentTime = 0;

            _levelStateMachine.OnUpdate += WaitForSeconds;
        }

        private void WaitForSeconds()
        {
            // if (MirrorTimerSyncer.Instance.isClient && !MirrorTimerSyncer.Instance.isServer)
            // {
            //     _initializeStateUI.SetWaitText($"Match starts in {MirrorTimerSyncer.Instance.LevelInitializeTimer}");
            //     return;
            // }

            _currentTime += Time.fixedDeltaTime;

            MirrorTimerSyncer.Instance.LevelInitializeTimer = _currentTime;

            _initializeStateUI.SetWaitText($"Match starts in {_levelSO.InitializeWaitTime - (int) _currentTime}");

            if (_currentTime < _levelSO.InitializeWaitTime)
                return;

            _levelStateMachine.SetState<GameplayState>();
        }

        public override void Exit()
        {
            _levelStateMachine.OnUpdate -= WaitForSeconds;
        }
    }
}