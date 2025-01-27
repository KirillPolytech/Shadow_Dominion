namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkLeftState : PlayerState
    {
        public WalkLeftState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }

        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationWalkLeftState>();
        }

        public override void Exit()
        {
        }
    }
}