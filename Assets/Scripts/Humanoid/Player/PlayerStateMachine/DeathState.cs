namespace Shadow_Dominion.Player.StateMachine
{
    public class DeathState : PlayerState
    {
        private CoroutineExecuter _coroutineExecuter;
        
        public DeathState(
            PlayerAnimation playerAnimation,
            CoroutineExecuter coroutineExecuter) : base(playerAnimation)
        {
            _coroutineExecuter = coroutineExecuter;
        }
        
        public override void Enter()
        {
            //copyTo[ind].IsPositionApplying(false);
            //copyTo[ind].IsRotationApplying(false);

        }

        public override void Exit()
        {
            
        }
    }
}