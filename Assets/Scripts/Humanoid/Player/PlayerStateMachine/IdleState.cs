namespace Shadow_Dominion.Player.StateMachine
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }

        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();
        }

        public override void Exit()
        {
        }
    }
}