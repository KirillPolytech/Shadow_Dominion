using TMPro;
using UnityEngine;

public class LevelPlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nick;
    [SerializeField] private TextMeshProUGUI kills;

    public void Initialize(string pNick, string pKills)
    {
        nick.text = pNick;
        kills.text = pKills;
    }
}