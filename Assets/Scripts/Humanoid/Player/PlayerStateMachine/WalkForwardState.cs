namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkForwardState : PlayerState
    {
        public WalkForwardState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationWalkForwardState>();
        }

        public override void Exit()
        {
        }
    }
}