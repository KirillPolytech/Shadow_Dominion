using System;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion.InputSystem
{
    public class InputHandler : IInputHandler, ITickable
    {
        public event Action<InputData> OnInputUpdate;

        private InputData _inputData;

        public void Tick()
        {
            HandleInput();
        }

        public void HandleInput()
        {
            _inputData.HorizontalAxisRaw = Input.GetAxisRaw(VariableNames.HorizontalAxis);
            _inputData.VerticalAxisRaw = Input.GetAxisRaw(VariableNames.VerticalAxis);

            _inputData.MouseX = Input.GetAxis(VariableNames.MouseX);
            _inputData.MouseX = Input.GetAxis(VariableNames.MouseY);

            _inputData.LeftShift = Input.GetKey(VariableNames.LeftShiftKey);
            _inputData.E = Input.GetKey(VariableNames.E);
            _inputData.E_Down = Input.GetKeyDown(VariableNames.E);
            _inputData.E_Up = Input.GetKeyUp(VariableNames.E);
            
            _inputData.T = Input.GetKey(VariableNames.T);
            _inputData.F_Down = Input.GetKeyDown(VariableNames.F);

            _inputData.LeftMouseButton = Input.GetMouseButton(VariableNames.LeftMouseButton);
            _inputData.LeftMouseButtonDown = Input.GetMouseButtonDown(VariableNames.LeftMouseButton);
            _inputData.LeftMouseButtonUp = Input.GetMouseButtonUp(VariableNames.LeftMouseButton);

            _inputData.RightMouseButton = Input.GetMouseButton(VariableNames.RightMouseButton);
            _inputData.RightMouseButtonDown = Input.GetMouseButtonDown(VariableNames.RightMouseButton);
            _inputData.RightMouseButtonUp = Input.GetMouseButtonUp(VariableNames.RightMouseButton);
            
            _inputData.MouseWheelScroll = Input.mouseScrollDelta.y;
            
            _inputData.ESC = Input.GetKeyDown(KeyCode.Escape);

            OnInputUpdate?.Invoke(_inputData);
        }
    }
}