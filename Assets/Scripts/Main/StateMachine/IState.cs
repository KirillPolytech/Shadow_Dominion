namespace Shadow_Dominion.Level.StateMachine
{
    public abstract class IState
    {
        public abstract void Enter();

        public abstract void Exit();
    }
}