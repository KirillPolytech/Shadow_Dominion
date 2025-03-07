using TMPro;
using Zenject;

public abstract class InputFieldsProvider
{
    public TMP_InputField TMPInputFields;

    [Inject]
    public InputFieldsProvider(TMP_InputField tmpInputFields)
    {
        TMPInputFields = tmpInputFields;
    }
}
