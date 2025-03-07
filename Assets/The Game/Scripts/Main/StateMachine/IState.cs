namespace Shadow_Dominion.StateMachine
{
    public abstract class IState
    {
        public abstract void Enter();
        
        public abstract void Exit();
    }
}