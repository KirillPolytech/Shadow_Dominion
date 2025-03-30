using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using WindowsSystem;

namespace Shadow_Dominion.StateMachine
{
    public class FinishState : IState
    {
        private readonly WindowsController _windowsController;
        
        public FinishState(WindowsController windowsController)
        {
            _windowsController = windowsController;
        }
        
        public override void Enter()
        {
            _windowsController.OpenWindow<FinishWindow>();
            CursorService.SetState(CursorLockMode.Confined);

            foreach (var player in MirrorServer.Instance.SpawnedPlayerInstances)
            {
                player.PlayerStateMachine.SetState<InActiveState>();
            }
        }

        public override void Exit()
        {

        }
    }
}