using Shadow_Dominion.AnimStateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkRightState : PlayerState
    {
        public WalkRightState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }

        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationWalkRightState>();
        }

        public override void Exit()
        {
        }
    }
}