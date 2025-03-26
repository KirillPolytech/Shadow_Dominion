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

            if (playerName.text != MirrorPlayersSyncer.Instance.LocalPlayer.Address)
            {
                Debug.LogWarning($"playerName.text {playerName.text} " +
                                 $"MirrorPlayersSyncer.Instance.LocalPlayer.Address " +
                                 $"{MirrorPlayersSyncer.Instance.LocalPlayer.Address}");
                return;
            }

            roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
        };

        readyButton.onClick.AddListener(_onButtonPressed);
        
        //removeButton.gameObject.SetActive();
        //removeButton.onClick.AddListener(() => );
    }

    private void OnDestroy()
    {
        readyButton.onClick.RemoveListener(_onButtonPressed);
    }

    public void SetName(string pName) => playerName.text = pName;

    public void SetButtonState(bool state)
    {
        _stateText.text = state ? _textSo.ViewReadyState : _textSo.ViewNotReadyState;

        Color color = state ? Color.green : Color.red;

        ColorBlock colors = readyButton.colors;
        colors.normalColor = color; // Цвет кнопки в обычном состоянии
        colors.highlightedColor = color; // Цвет при наведении
        colors.pressedColor = color; // Цвет при нажатии
        colors.selectedColor = color; // Цвет при выборе
        readyButton.colors = colors;
    }
}