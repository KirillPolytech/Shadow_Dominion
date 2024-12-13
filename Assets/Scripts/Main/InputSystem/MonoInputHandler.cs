using System;
using UnityEngine;

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

        _inputData.LeftShift = Input.GetKeyDown(VariableNames.LeftShiftKey);

        OnInputUpdate?.Invoke(_inputData);
    }
}