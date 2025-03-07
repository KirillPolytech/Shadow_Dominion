using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    
    [SerializeField]
    private Image checkmark;

    public void SetName(string pName) => playerName.text = pName;
    
    public void SetCheckMarkState(bool state) => checkmark.enabled = state;
}
