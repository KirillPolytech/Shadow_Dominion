using System;
using UnityEngine;

namespace HellBeavers
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

            OnInputUpdate?.Invoke(_inputData);
        }
    }
}