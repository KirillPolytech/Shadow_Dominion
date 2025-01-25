namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkBackwardState : PlayerState
    {
        public WalkBackwardState(PlayerAnimation playerAnimation) : base(playerAnimation)
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