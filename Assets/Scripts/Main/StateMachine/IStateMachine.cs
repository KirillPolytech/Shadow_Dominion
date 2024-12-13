using System.Collections.Generic;

namespace HellBeavers.Level.StateMachine
{
    public abstract class IStateMachine
    {
        public IState CurrentState { get; protected set; }

        protected List<IState> _states = new List<IState>();

        public abstract void SetState<T>();
    }
}