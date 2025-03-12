using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorServer : NetworkRoomManager
    {
        public static MirrorServer Instance;

        // Server only
        public readonly List<NetworkConnectionToClient> Connections = new List<NetworkConnectionToClient>();
        
        public event Action<HashSet<NetworkRoomPlayer>> OnPlayerReadyChanged;

        public List<Main.Player> SpawnedPlayerInstances = new List<Main.Player>();

        [SerializeField]
        private MirrorPlayersSyncer mirrorPlayerStateSyncerPrefab;
        
        public event Action ActionOnHostStart;
        public event Action ActionOnHostStop;
        public event Action ActionOnServerAddPlayer;
        public event Action<NetworkConnectionToClient> ActionOnServerConnectWithArg;
        public event Action ActionOnServerConnect;
        public event Action<NetworkConnectionToClient> ActionOnServerDisconnectWithArg;
        public event Action ActionOnServerDisconnect;

        public event Action ActionOnStartClient;
        public event Action ActionOnStopClient;
        public event Action ActionOnClientConnect;
        public event Action ActionOnClientDisconnect;
        
        public event Action ActionOnAnyChange;

        private Action<NetworkConnection> _cachedRemove;

        private PositionMessage[] _positionMessages;
        private int _ind;

        [Inject]
        public void Construct(PositionMessage[] positionMessages)
        {
            _positionMessages = positionMessages;
        }
        
        public override void Awake()
        {
            base.Awake();

            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            ActionOnHostStart += SpawnBehaviours;
            
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

        private void SpawnBehaviours()
        {
            NetworkServer.Spawn(Instantiate(mirrorPlayerStateSyncerPrefab.gameObject));
        }
        
        private void OnAnyChange() => ActionOnAnyChange?.Invoke();

        public override void OnDestroy()
        {
            ActionOnHostStart -= SpawnBehaviours;

            ActionOnHostStart -= OnAnyChange;
            ActionOnHostStop -= OnAnyChange;
            ActionOnServerAddPlayer -= OnAnyChange;
            ActionOnServerDisconnect -= OnAnyChange;

            ActionOnStartClient -= OnAnyChange;
            ActionOnStopClient -= OnAnyChange;
            ActionOnClientConnect -= OnAnyChange;
            ActionOnClientDisconnect -= OnAnyChange;
            
            DontDestroyOnLoad(gameObject);
        }

        #region Server

        [Server]
        public override void OnStartHost()
        {
            base.OnStartHost();

            ActionOnHostStart?.Invoke();

            Debug.Log($"[Server] OnStartHost {networkAddress}");
        }

        [Server]
        public override void OnStopHost()
        {
            base.OnStopHost();

            ActionOnHostStop?.Invoke();

            Debug.Log($"[Server] OnStopHost");
        }

        [Server]
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            ActionOnServerAddPlayer?.Invoke();

            Debug.Log($"[Server] OnServerAddPlayer. {conn.address}");
        }

        [Server]
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            ActionOnServerConnect?.Invoke();
            ActionOnServerConnectWithArg?.Invoke(conn);
            
            Connections.Add(conn);

            Debug.Log($"[Server] OnServerConnect. {conn.address}");
        }

        [Server]
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            ActionOnServerDisconnect?.Invoke();
            ActionOnServerDisconnectWithArg?.Invoke(conn);

            Connections.Remove(conn);

            Debug.Log($"[Server] OnServerDisconnect. {conn.address}");
        }

        [Server]
        public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
        {
            Debug.LogError($"[Server] OnServerError {conn}, {error}, {reason}");
        }

        [Server]
        public override void OnServerTransportException(NetworkConnectionToClient conn, Exception exception)
        {
            base.OnServerTransportException(conn, exception);

            Debug.LogError($"[Server] OnServerTransportException {exception.Message}");
        }

        [Server]
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, 
            GameObject roomPlayer, GameObject gamePlayer)
        {
            Main.Player player = gamePlayer.GetComponent<Main.Player>();
            
            StartCoroutine(WaitForInitialization(player));
            
            Debug.Log($"[Server] OnRoomServerSceneLoadedForPlayer {conn.address}");

            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }

        private IEnumerator WaitForInitialization(Main.Player player)
        {
            while (player.PlayerStateMachine == null)
            {
                yield return new WaitForSeconds(1);
            }
            
            yield return new WaitForSeconds(1);

            player.RpcUpdateState(typeof(InActiveState).ToString());
        }

        public override void ReadyStatusChanged()
        {
            base.ReadyStatusChanged();
            
            OnPlayerReadyChanged?.Invoke(roomSlots);
        }

        #endregion

        #region Client
        [Client]
        public override void OnStartClient()
        {
            base.OnStartClient();

            ActionOnStartClient?.Invoke();

            // Debug.Log($"OnStartClient: networkAddress:{networkAddress}");
        }
        
        [Client]
        public override void OnStopClient()
        {
            base.OnStopClient();

            ActionOnStopClient?.Invoke();

            // Debug.Log($"OnStartClient: networkAddress:{networkAddress}");
        }
        
        [Client]
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            ActionOnClientConnect?.Invoke();

            // Debug.Log($"OnClientConnect: networkAddress:{networkAddress}");
        }

        [Client]
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            ActionOnClientDisconnect?.Invoke();

            // Debug.Log($"OnClientDisconnect: networkAddress:{networkAddress}");
        }
        
        [Client]
        public override void OnClientError(TransportError error, string reason)
        {
            base.OnClientError(error, reason);
            
            Debug.LogError($"OnClientError {error}, {reason}");
        }
        
        [Client]
        public override void OnClientTransportException(Exception exception)
        {
            base.OnClientTransportException(exception);

            Debug.LogError($"[Client] OnClientTransportException {exception.Message}");
        }
        
        #endregion
    }
}