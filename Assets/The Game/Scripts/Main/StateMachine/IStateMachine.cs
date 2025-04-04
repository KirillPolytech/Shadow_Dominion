using System.Collections.Generic;

namespace Shadow_Dominion.StateMachine
{
    public abstract class IStateMachine
    {
        protected readonly List<IState> _states = new List<IState>();
        
        public IState CurrentState { get; protected set; }
        
        public abstract void SetState<T>();
    }
}