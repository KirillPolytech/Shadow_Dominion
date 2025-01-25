namespace Shadow_Dominion.Player.StateMachine
{
    public class RunBackwardState : PlayerState
    {
        public RunBackwardState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationRunBackwardState>();
        }

        public override void Exit()
        {
        }
    }
}