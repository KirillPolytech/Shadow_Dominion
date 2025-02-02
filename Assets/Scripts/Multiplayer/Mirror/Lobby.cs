using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class Lobby : NetworkBehaviour
{
    private readonly SyncList<string> _playerNames = new SyncList<string>();

    public Action<List<string>> OnChange;

    [SerializeField]
    private MirrorServer _mirrorServer;

    private string _temp;

    private Action<SyncList<string>.Operation, int, string> _onChange;

    private void Start()
    {
        Debug.Log("Lobby created.");

        _mirrorServer = FindAnyObjectByType<MirrorServer>();
        PlayerListing playerListing = FindAnyObjectByType<PlayerListing>();

        _onChange += (op, index, newItem) =>
        {
            Debug.Log($"SyncList changed: {op}, {index}, {newItem}");
            OnChange?.Invoke(_playerNames.ToList());
            
            playerListing.UpdateUI(_playerNames.ToList());
        };

        _playerNames.OnChange += _onChange.Invoke;
        _mirrorServer.ActionOnHostStart += UpdateList;
        _mirrorServer.ActionOnServerAddPlayer += UpdateList;
    }

    private void OnDestroy()
    {
        _playerNames.OnChange -= _onChange;
        _mirrorServer.ActionOnHostStart -= UpdateList;
        _mirrorServer.ActionOnServerAddPlayer -= UpdateList;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdateList();
        }
    }

    private void UpdateList()
    {
        _playerNames.Clear();
        foreach (var conn in NetworkServer.connections.Values)
        {
            _playerNames.Add(conn.address);
        }

        //Debug.Log($"List updated: {_syncNames}");
    }
}