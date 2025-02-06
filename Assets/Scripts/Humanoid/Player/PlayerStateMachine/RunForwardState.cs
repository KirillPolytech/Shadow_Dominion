using Shadow_Dominion.AnimStateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class RunForwardState : PlayerState
    {
        public RunForwardState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }

        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationRunForwardState>();
        }

        public override void Exit()
        {
        }
    }
}