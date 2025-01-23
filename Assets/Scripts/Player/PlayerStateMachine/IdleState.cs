using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class IdleState : IState
    {
        private PlayerMovement _playerMovement;
        private PlayerAnimation _playerAnimation;
        
        public IdleState(PlayerMovement playerMovement, PlayerAnimation playerAnimation)
        {
            _playerMovement = playerMovement;
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