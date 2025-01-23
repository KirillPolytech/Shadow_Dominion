using UnityEngine;

namespace Shadow_Dominion.StateMachine
{
    public class GameplayState : IState
    {
        private readonly CursorService _cursorService;

        public GameplayState(CursorService cursorService)
        {
            _cursorService = cursorService;
        }

        public override void Enter()
        {
            _cursorService.SetState(CursorLockMode.Locked);
        }

        public override void Exit()
        {
        }
    }
}