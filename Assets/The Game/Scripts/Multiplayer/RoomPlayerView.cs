using System.Linq;
using Mirror;
using Shadow_Dominion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class RoomPlayerView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    
    [SerializeField]
    private Button button;
    
    private UnityAction _onButtonPressed;
    private TextMeshProUGUI _stateText;
    private TextSO _textSo;

    [Inject]
    public void Construct(TextSO textSo)
    {
        _textSo = textSo;
    }
    
    private void Awake()
    {
        _stateText = button.GetComponentInChildren<TextMeshProUGUI>();
        
        _onButtonPressed = () =>
        {
            NetworkConnectionToClient localPlayer = MirrorPlayersSyncer.Instance.Connections.First(x => x.identity.isClient);
            
            if (playerName.text != localPlayer.address)
                return;
            
            MirrorPlayersSyncer.Instance.CmdChangeState(localPlayer.identity.GetComponent<NetworkRoomPlayer>());
        };
        
        button.onClick.AddListener(_onButtonPressed);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(_onButtonPressed);
    }

    public void SetName(string pName) => playerName.text = pName;
    
    public void SetButtonState(bool state) 
    {
        _stateText.text = state ? _textSo.ViewReadyState : _textSo.ViewNotReadyState;

        Color color = state ? Color.green : Color.red;
        
        ColorBlock colors = button.colors;
        colors.normalColor = color; // Цвет кнопки в обычном состоянии
        colors.highlightedColor = color; // Цвет при наведении
        colors.pressedColor = color; // Цвет при нажатии
        colors.selectedColor = color; // Цвет при выборе
        button.colors = colors; 
    }
}
