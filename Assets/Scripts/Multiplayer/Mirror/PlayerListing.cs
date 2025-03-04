using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviour
{
    private readonly List<Transform> _ui = new List<Transform>();

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private TextMeshProUGUI prefab;
    
    private void Start()
    {
        Debug.Log("PlayerListing created.");
    }

    public void UpdateUI(string[] players)
    {
        Clear();

        if (players == null)
            return;

        string currentPlayers = "players:\n";

        foreach (var VARIABLE in players)
        {
            TextMeshProUGUI playerName = Instantiate(prefab, parent);
            playerName.text = $"{VARIABLE}";
            _ui.Add(playerName.transform);

            currentPlayers += $"{VARIABLE}\n";
        }

        // Debug.Log("Player list updated " + currentPlayers);
    }

    private void Clear()
    {
        for (int i = 0; i < _ui.Count; i++)
        {
            Destroy(_ui.ElementAt(i).gameObject);
        }

        _ui.Clear();
    }
}