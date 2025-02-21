using Shadow_Dominion;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private RectTransform roomViewPrefab;

    private void OnEnable()
    {
        MirrorLobby.Instance.OnPlayerReadyWithAddress += SpawnView;
    }

    private void OnDestroy()
    {
        MirrorLobby.Instance.OnPlayerReadyWithAddress -= SpawnView;
    }

    private void SpawnView(string address)
    {
        Transform instance = Instantiate(roomViewPrefab.transform, content);

        instance.GetComponent<TextMeshProUGUI>().text = address;
    }
}