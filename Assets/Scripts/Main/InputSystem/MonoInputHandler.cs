using System;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MonoInputHandler : MonoBehaviour
    {
        public event Action<InputData> OnInputUpdate;

        private InputData _inputData;

        private void Update()
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

            OnInputUpdate?.Invoke(_inputData);
        }
    }
}