using System.Linq;
using Mirror;
using UnityEngine;

public class Lobby : NetworkBehaviour
{
    private readonly SyncList<string> _playerNames = new SyncList<string>();

    private void OnEnable()
    {
        PlayerListing playerListing = FindAnyObjectByType<PlayerListing>();

        _playerNames.OnChange += (a,b,c) => playerListing.UpdateUI(_playerNames.ToArray());
    }

    [Server]
    public void UpdateList(string[] names)
    {
        _playerNames.Clear();
        _playerNames.AddRange(names);

        Debug.Log($"List updated");
    }
}