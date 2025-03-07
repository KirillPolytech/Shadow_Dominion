using TMPro;
using UnityEngine;

public class InitializeStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waitText;
    
    public void SetWaitText(string text)
    {
        waitText.text = text;
    }
}
