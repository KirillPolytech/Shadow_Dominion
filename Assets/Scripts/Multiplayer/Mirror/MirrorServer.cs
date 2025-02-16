using System;
using System.Linq;
using Shadow_Dominion;
using Mirror;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class MirrorServer : NetworkManager
{
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
    
    [SerializeField]
    private Lobby lobby;
    
    private Lobby _lobby;

    private PlayerPool _playerPool;

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

        ActionOnServerConnect += SpawnNetworkBehaviours;
        ActionOnServerConnect += UpdateListing;
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

        ActionOnServerConnect -= SpawnNetworkBehaviours;
        ActionOnServerConnect -= UpdateListing;
    }

    [Inject]
    public void Construct(PlayerPool playerPool)
    {
        _playerPool = playerPool;
    }

    public override void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown(KeyCode.J))
            UpdateListing();
        
        if (Input.GetKeyDown(KeyCode.K))
            SpawnNetworkBehaviours();
        
        if (Input.GetKeyDown(KeyCode.L))
            DestroyNetworkBehaviours();
    }

    private void UpdateListing()
    {
        string[] names = NetworkServer.connections
            .Select(x => x.Value.address).ToArray();
        _lobby.UpdateList(names);
    }
    
    [Server]
    private void SpawnNetworkBehaviours()
    {
        DestroyNetworkBehaviours();
        
        _lobby = Instantiate(lobby);
        NetworkServer.Spawn(_lobby.gameObject);
        
        Debug.Log("Objects spawned!");
    }

    [Server]
    private void DestroyNetworkBehaviours()
    {
        if (_lobby)
        {
            NetworkServer.Destroy(_lobby.gameObject);
            Destroy(_lobby.gameObject);
        }
        
        Debug.Log("Objects destroyed!");
    }

    private void OnAnyChange() => ActionOnAnyChange?.Invoke();

    [Server]
    private void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
    {
        //локально на сервере создаем gameObject
        GameObject go = _playerPool.Pull().gameObject;
        go.transform.SetPositionAndRotation(positionMessage.pos, Quaternion.identity);
        //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
        NetworkServer.AddPlayerForConnection(conn, go);
        Debug.Log($"OnCreateCharacter: {conn.address}");
    }

    private void ActivatePlayerSpawn()
    {
        float value = Random.Range(-5, 5);
        Vector3 newpos = new Vector3(value, 2, value);
        //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся
        PositionMessage message = new PositionMessage { pos = newpos };
        //отправка сообщения на сервер с координатами спавна
        NetworkClient.Send(message);
    }

    [Server]
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

    [Server]
    public override void OnStartHost()
    {
        base.OnStartHost();

        //указываем, какой struct должен прийти на сервер, чтобы выполнился свапн
        NetworkServer.RegisterHandler<PositionMessage>(OnCreateCharacter);

        ActionOnHostStart?.Invoke();

        // Debug.Log($"OnStartHost: networkAddress:{networkAddress}");
    }

    [Server]
    public override void OnStopHost()
    {
        base.OnStopHost();

        ActionOnHostStop?.Invoke();

        //Debug.Log($"OnStopHost.");
    }

    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        ActionOnServerAddPlayer?.Invoke();

        //Debug.Log($"OnServerAddPlayer. {conn.address}");
    }

    [Server]
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        ActionOnServerConnect?.Invoke();
        ActionOnServerConnectWithArg?.Invoke(conn);

        //NetworkRoomPlayer roomPrefab = Instantiate(roomPlayerPrefab);
        //NetworkServer.AddPlayerForConnection(conn, roomPrefab.gameObject);

        //Debug.Log($"OnServerConnect. {conn.address}");
    }
/*
    [Server]
    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);

        Debug.Log($"OnRoomServerAddPlayer. {conn.address}");
    }

    [Server]
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer,
        GameObject gamePlayer)
    {
        Debug.Log($"OnRoomServerSceneLoadedForPlayer {conn.address}");

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }
    */

    [Server]
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        ActionOnServerDisconnect?.Invoke();
        ActionOnServerDisconnectWithArg?.Invoke(conn);

        //Debug.Log($"OnServerDisconnect. {conn.address}");
    }

    [Server]
    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
    {
        Debug.LogError($"OnServerError {conn}, {error}, {reason}");
    }

    [Server]
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

    [Server]
    public override void OnClientError(TransportError error, string reason)
    {
        Debug.LogError($"OnClientError {error}, {reason}");
    }

    [Server]
    public override void OnClientTransportException(Exception exception)
    {
        base.OnClientTransportException(exception);

        Debug.LogError($"OnClientTransportException {exception.Message}");
    }
}

public struct PositionMessage : NetworkMessage
{
    public Vector3 pos;
}