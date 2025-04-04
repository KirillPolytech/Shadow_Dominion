using System;
using Shadow_Dominion.InputSystem;

namespace Shadow_Dominion.StateMachine
{
    public abstract class MovementState : IState
    {
        public abstract void Update(InputData inputData);

        public abstract bool CanExit();
    }
}