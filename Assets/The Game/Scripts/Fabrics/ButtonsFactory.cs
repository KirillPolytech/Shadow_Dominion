using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsFactory
{
    public Button Create(Button prefab, [Optional] Transform parent, [Optional] string objectName)
    {
        Button button = Object.Instantiate(prefab, parent);
        button.gameObject.name = objectName ?? "Button";
        
        return button;
    }
}