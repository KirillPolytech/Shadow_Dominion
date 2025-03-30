using TMPro;
using Zenject;

public abstract class InputFieldsProvider
{
    public readonly TMP_InputField TMPInputField;
    
    [Inject]
    public InputFieldsProvider(TMP_InputField tmpInputField)
    {
        TMPInputField = tmpInputField;
    }
}
