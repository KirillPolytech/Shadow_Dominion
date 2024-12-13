using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class Lobby : NetworkBehaviour
{
    //private readonly SyncList<string> _playerNames = new SyncList<string>();
    public Action<List<string>> OnChange;

    [SerializeField] private MirrorServer _mirrorServer;

    //задаем метод, который будет выполняться при синхронизации переменной
    [SyncVar]
    private string _syncNames = string.Empty;

    private string _temp;

    private Action<SyncList<string>.Operation, int, string> _onChange;

    public void SyncNames(string oldValue, string newValue) 
    {
        _syncNames = newValue;
        
        OnChange?.Invoke(_syncNames?.Split("\n").ToList());
        
        Debug.Log("Server: names synced");
    }

    private void FixedUpdate()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            UpdateList();
        
        if (_syncNames == _temp)
            return;
        
        Debug.Log($"_syncNames: {_syncNames}");

        _temp = _syncNames;
    }

    //[Inject]
    public void Start()
    {
        // _onChange += (op, index, newItem) =>
        // {
        //     Debug.Log($"SyncList changed: {op}, {index}, {newItem}");
        //     OnChange?.Invoke(_playerNames.ToList());
        // };
        
        _mirrorServer.ActionOnHostStart += OnStartServer;
        _mirrorServer.ActionOnHostStop += OnStopServer;

        _mirrorServer.ActionOnStartClient += OnStartClient;
        _mirrorServer.ActionOnStopClient += OnStopClient;
        
        _mirrorServer.ActionOnHostStop += () => OnChange?.Invoke(null);
        _mirrorServer.ActionOnStopClient += () => OnChange?.Invoke(null);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Подписка на события подключения/отключения клиентов
        _mirrorServer.ActionOnServerConnectWithArg += OnClientConnected;
        _mirrorServer.ActionOnServerDisconnectWithArg += OnClientDisconnected;

        UpdateList();
        
        // Debug.Log("Server started and subscribed to client events.");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        _mirrorServer.ActionOnServerConnectWithArg -= OnClientConnected;
        _mirrorServer.ActionOnServerDisconnectWithArg -= OnClientDisconnected;

        // Debug.Log("Server stopped and unsubscribed from client events.");
    }
    
    public override void OnStartClient()
    {
        //_playerNames.OnChange += _onChange.Invoke;

        // Debug.Log("Client started and subscribed to SyncList changes.");
    }

    public override void OnStopClient()
    {
        //_playerNames.OnChange -= _onChange.Invoke;

        // Debug.Log("Client stopped and unsubscribed from SyncList changes.");
    }

    [Server]
    private void OnClientConnected(NetworkConnection conn)
    {
        UpdateList();
        
        // Debug.Log($"Client connected: {conn.connectionId}");
    }

    [Server]
    private void OnClientDisconnected(NetworkConnection conn)
    {
        UpdateList();
        // Debug.Log($"Client disconnected: {conn.connectionId}");
    }

    [Server]
    private void UpdateList()
    {
        // _playerNames.Clear();
        // foreach (var conn in NetworkServer.connections.Values)
        // {
        //     _playerNames.Add(conn.address);
        // }

        string temp = default;
        foreach (var VARIABLE in NetworkServer.connections.Values)
        {
            temp += $"{VARIABLE.address}\n";
        }
        _syncNames = temp;
        
        //Debug.Log($"List updated: {_syncNames}");
    }

    private void OnDestroy()
    {
        _mirrorServer.ActionOnHostStart -= OnStartServer;
        _mirrorServer.ActionOnHostStop -= OnStopServer;

        _mirrorServer.ActionOnStartClient -= OnStartClient;
        _mirrorServer.ActionOnStopClient -= OnStopClient;
    }
}