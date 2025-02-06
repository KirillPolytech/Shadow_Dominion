using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public abstract class PlayerState : IState
    {
        protected readonly PlayerAnimation _playerAnimation;

        public PlayerState(PlayerAnimation playerAnimation)
        {
            _playerAnimation = playerAnimation;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}