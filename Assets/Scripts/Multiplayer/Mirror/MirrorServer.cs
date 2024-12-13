using System;
using Mirror;
using UnityEngine;

public class MirrorServer : NetworkRoomManager
{
    [SerializeField] private Lobby lobby;
    
    public event Action ActionOnHostStart;
    public event Action ActionOnHostStop;
    public event Action ActionOnServerAddPlayer;
    public event Action<NetworkConnection> ActionOnServerConnectWithArg;
    public event Action ActionOnServerConnect;
    public event Action<NetworkConnection> ActionOnServerDisconnectWithArg;
    public event Action ActionOnServerDisconnect;

    public event Action ActionOnStartClient;
    public event Action ActionOnStopClient;
    public event Action ActionOnClientConnect;
    public event Action ActionOnClientDisconnect;

    public event Action ActionOnAnyChange;

    public override void Awake()
    {
        base.Awake();
        
        ActionOnHostStart += OnAnyChange;
        ActionOnHostStop += OnAnyChange;
        ActionOnServerAddPlayer += OnAnyChange;
        ActionOnServerConnect += OnAnyChange;
        ActionOnServerDisconnect += OnAnyChange;

        ActionOnStartClient += OnAnyChange;
        ActionOnStopClient += OnAnyChange;
        ActionOnClientConnect += OnAnyChange;
        ActionOnClientDisconnect += OnAnyChange;
    }

    private void OnAnyChange() => ActionOnAnyChange?.Invoke();

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        
        Debug.Log($"OnServerSceneChanged: {sceneName}");
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        ActivatePlayerSpawn();
        Debug.Log($"OnClientChangeScene: {newSceneName}");
    }

    public void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
    {
        //локально на сервере создаем gameObject
        GameObject go = Instantiate(playerPrefab, positionMessage.pos, Quaternion.identity);
        //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
        NetworkServer.AddPlayerForConnection(conn, go);
        Debug.Log($"OnCreateCharacter: {conn.address}");
    }
    
    public void ActivatePlayerSpawn()
    {
        //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся
        PositionMessage message = new PositionMessage { pos = Vector3.zero };
        //отправка сообщения на сервер с координатами спавна
        NetworkClient.Send(message);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        
        //указываем, какой struct должен прийти на сервер, чтобы выполнился свапн
        NetworkServer.RegisterHandler<PositionMessage>(OnCreateCharacter);

        ActionOnHostStart?.Invoke();

       // Debug.Log($"OnStartHost: networkAddress:{networkAddress}");
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        
        ActionOnHostStop?.Invoke();
        
        //Debug.Log($"OnStopHost.");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        ActionOnServerAddPlayer?.Invoke();

        //Debug.Log($"OnServerAddPlayer. {conn.address}");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        
        ActionOnServerConnect?.Invoke();
        ActionOnServerConnectWithArg?.Invoke(conn);

        //Debug.Log($"OnServerConnect. {conn.address}");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        ActionOnServerDisconnect?.Invoke();
        ActionOnServerDisconnectWithArg?.Invoke(conn);

        //Debug.Log($"OnServerDisconnect. {conn.address}");
    }

    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
    {
        Debug.LogError($"OnServerError {conn}, {error}, {reason}");
    }

    public override void OnServerTransportException(NetworkConnectionToClient conn, Exception exception)
    {
        base.OnServerTransportException(conn, exception);
        
        Debug.LogError($"OnServerTransportException {exception.Message}");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        ActionOnStartClient?.Invoke();

        // Debug.Log($"OnStartClient: networkAddress:{networkAddress}");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        
        ActionOnStopClient?.Invoke();
        
        // Debug.Log($"OnStartClient: networkAddress:{networkAddress}");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        ActionOnClientConnect?.Invoke();

        // Debug.Log($"OnClientConnect: networkAddress:{networkAddress}");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ActionOnClientDisconnect?.Invoke();

        // Debug.Log($"OnClientDisconnect: networkAddress:{networkAddress}");
    }

    public override void OnClientError(TransportError error, string reason)
    {
        Debug.LogError($"OnClientError {error}, {reason}");
    }

    public override void OnClientTransportException(Exception exception)
    {
        base.OnClientTransportException(exception);
        
        Debug.LogError($"OnClientTransportException {exception.Message}");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        ActionOnHostStart -= OnAnyChange;
        ActionOnHostStop -= OnAnyChange;
        ActionOnServerAddPlayer -= OnAnyChange;
        ActionOnServerDisconnect -= OnAnyChange;

        ActionOnStartClient -= OnAnyChange;
        ActionOnStopClient -= OnAnyChange;
        ActionOnClientConnect -= OnAnyChange;
        ActionOnClientDisconnect -= OnAnyChange;
    }
}

public struct PositionMessage : NetworkMessage
{
    public Vector3 pos;
}