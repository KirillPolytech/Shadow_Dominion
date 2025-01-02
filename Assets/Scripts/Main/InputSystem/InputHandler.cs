using System;
using UnityEngine;
using Zenject;

namespace HellBeavers
{
    public class InputHandler : ITickable
    {
        public event Action<InputData> OnInputUpdate;

        private InputData _inputData;

        public void Tick()
        {
            _inputData.HorizontalAxisRaw = Input.GetAxisRaw(VariableNames.HorizontalAxis);
            _inputData.VerticalAxisRaw = Input.GetAxisRaw(VariableNames.VerticalAxis);

            _inputData.MouseX = Input.GetAxis(VariableNames.MouseX);
            _inputData.MouseX = Input.GetAxis(VariableNames.MouseY);

            _inputData.LeftShift = Input.GetKey(VariableNames.LeftShiftKey);

            OnInputUpdate?.Invoke(_inputData);
        }
    }
}