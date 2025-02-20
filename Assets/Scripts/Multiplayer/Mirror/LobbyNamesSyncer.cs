using System;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

public class LobbyNamesSyncer : NetworkBehaviour
{
    private readonly Action<SyncList<string>.Operation, int, string> _cached;
    
    private PlayerListing _playerListing;

    public void Awake()
    {
        _playerListing = Object.FindAnyObjectByType<PlayerListing>();

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    [ClientRpc]
    public void UpdateList(string[] names)
    {
        _playerListing.UpdateUI(names);
        
        Debug.Log($"List updated {string.Join(",", names)}");
    }
}