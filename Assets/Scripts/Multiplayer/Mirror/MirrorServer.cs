using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorServer : NetworkRoomManager
    {
        private readonly List<NetworkConnection> _players = new List<NetworkConnection>();

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
        public event Action ActionOnClientChangeScene;
        
        public event Action<NetworkConnectionToClient> ActionOnRoomServerAddedPlayerWithArg;

        public event Action ActionOnAnyChange;

        private Action<NetworkConnection> _cachedRemove;

        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            ActionOnHostStart += OnAnyChange;
            ActionOnHostStop += OnAnyChange;
            ActionOnServerAddPlayer += OnAnyChange;
            ActionOnServerConnect += OnAnyChange;
            ActionOnServerDisconnect += OnAnyChange;

            ActionOnStartClient += OnAnyChange;
            ActionOnStopClient += OnAnyChange;
            ActionOnClientConnect += OnAnyChange;
            ActionOnClientDisconnect += OnAnyChange;
            
            ActionOnServerConnectWithArg += _players.Add;

            _cachedRemove = con => _players.Remove(con);
            ActionOnServerDisconnectWithArg += _cachedRemove;
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

            ActionOnServerConnectWithArg -= _players.Add;
            ActionOnServerDisconnectWithArg -= _cachedRemove;
        }

        private void OnAnyChange() => ActionOnAnyChange?.Invoke();

        [Server]
        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);

            Debug.Log($"OnServerSceneChanged: {sceneName}");
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation,
            bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

            ActionOnClientChangeScene?.Invoke();

            // Debug.Log($"OnClientChangeScene: {newSceneName}");
        }

        [Server]
        public override void OnStartHost()
        {
            base.OnStartHost();

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
            
            //Debug.Log($"OnServerConnect. {conn.address}");
        }

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

        [Server]
        public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnRoomServerAddPlayer(conn);
            
            ActionOnRoomServerAddedPlayerWithArg?.Invoke(conn);
            
            Debug.Log($"OnRoomServerAddPlayer. {conn.address}");
        }

        [Server]
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer,
            GameObject gamePlayer)
        {
            Debug.Log($"OnRoomServerSceneLoadedForPlayer {conn.address}");

            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }
    }

    [Serializable]
    public struct PositionMessage : NetworkMessage
    {
        public Vector3 pos;
        public bool isFree;
    }
}