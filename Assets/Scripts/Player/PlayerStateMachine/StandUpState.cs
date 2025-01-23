using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpState : IState
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimation _playerAnimation;
        
        public StandUpState(PlayerMovement playerMovement, PlayerAnimation playerAnimation)
        {
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<StandUpState>();
            _playerMovement.CanMove = false;
        }

        public override void Exit()
        {
            
        }
    }
}