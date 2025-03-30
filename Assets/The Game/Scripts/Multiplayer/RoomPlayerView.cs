using System;
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
    [SerializeField] private TextMeshProUGUI playerName;

    [SerializeField] private Button readyButton;
    [SerializeField] private Button removeButton;

    private UnityAction _onButtonPressed;
    private TextMeshProUGUI _stateText;
    private TextSO _textSo;
    private Action _cachedRemove;
    
    [Inject]
    public void Construct(TextSO textSo)
    {
        _textSo = textSo;
    }

    private void Awake()
    {
        _stateText = readyButton.GetComponentInChildren<TextMeshProUGUI>();

        _onButtonPressed = () =>
        {
            NetworkRoomPlayer roomPlayer =
                FindObjectsByType<NetworkRoomPlayer>(FindObjectsSortMode.None).First(x => x.isLocalPlayer);

            if (playerName.text != MirrorPlayersSyncer.Instance.LocalPlayer.Nick)
            {
                Debug.LogWarning($"playerName.text {playerName.text}");
                return;
            }
            
            roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
            
            //Debug.Log("[Client] ButtonPressed");
        };
    }

    public void SetName(string pName) => playerName.text = pName;

    public void SetButtonState(bool state)
    {
        _stateText.text = state ? _textSo.ViewReadyState : _textSo.ViewNotReadyState;

        Color color = state ? Color.green : Color.red;

        ColorBlock colors = readyButton.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color; 
        colors.selectedColor = color;
        readyButton.colors = colors;
    }
}