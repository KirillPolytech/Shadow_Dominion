using Shadow_Dominion.AnimStateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class WalkDiagonallyRightState : PlayerState
    {
        public WalkDiagonallyRightState(PlayerAnimation playerAnimation) : base(playerAnimation)
        {
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.Reset();
            _playerAnimation.AnimationStateMachine.SetState<AnimationWalkDiagonallyRightState>();
        }

        public override void Exit()
        {
        }
    }
}