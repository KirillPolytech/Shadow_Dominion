using Shadow_Dominion.AnimStateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkDiagonallyLeftState : PlayerState
    {
        public WalkDiagonallyLeftState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }

        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.Reset();
            _playerAnimation.AnimationStateMachine.SetState<AnimationWalkDiagonallyLeftState>();
        }

        public override void Exit()
        {
        }
    }
}