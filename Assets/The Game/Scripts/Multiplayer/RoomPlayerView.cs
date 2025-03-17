using System.Linq;
using Mirror;
using Shadow_Dominion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomPlayerView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    
    [SerializeField]
    private Button button;

    private UnityAction _onButtonPressed;
    
    private void Awake()
    {
        _onButtonPressed = () =>
        {
            NetworkConnectionToClient localPlayer = MirrorServer.Instance.Connections.First(x => x.identity.isClient);
            
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
    
    public void SetButtonState(bool state) => button.GetComponentInChildren<TextMeshProUGUI>().text = state ? "Ready" : "Not ready";
}
