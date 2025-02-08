using System;

namespace Shadow_Dominion.InputSystem
{
    public interface IInputHandler
    {
        public event Action<InputData> OnInputUpdate;

        public void HandleInput();
    }
}