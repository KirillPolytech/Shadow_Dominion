namespace HellBeavers.Level.StateMachine
{
    public abstract class IState
    {
        public abstract void Enter();

        public abstract void Exit();
    }
}