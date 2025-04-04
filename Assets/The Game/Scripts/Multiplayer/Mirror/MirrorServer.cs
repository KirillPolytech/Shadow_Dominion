using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Shadow_Dominion.Main;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shadow_Dominion
{
    public class MirrorServer : NetworkRoomManager
    {
        public static MirrorServer Instance { get; private set; }
        
        // Server only
        public readonly List<NetworkConnectionToClient> Connections = new();
        public readonly List<PlayerViewData> PlayerViewData = new();
        public readonly List<MirrorPlayer> SpawnedPlayerInstances = new();
        //
        public event Action<List<NetworkConnectionToClient>> OnPlayerReadyChanged;
        public event Action OnPlayerLoadedOnLevel;
        public event Action<NetworkConnectionToClient> OnPlayerLoadedOnLevelWithArg;
        
        [SerializeField]
        private MirrorPlayersSyncer mirrorPlayerStateSyncerPrefab;
        
        [SerializeField]
        private SpawnPointSyncer spawnPointSyncerPrefab;
        
        [FormerlySerializedAs("gameStateManager")] [SerializeField]
        private GameStateManager gameStateManagerPrefab;
        
        [SerializeField]
        private KillFeedSyncer killFeedSyncerPrefab;
        
        [SerializeField]
        private MirrorTimerSyncer mirrorTimerSyncerPrefab;
        
        public event Action ActionOnHostStart;
        public event Action ActionOnHostStop;
        public event Action ActionOnServerAddPlayer;
        public event Action<NetworkConnectionToClient> ActionOnServerConnectWithArg;
        public event Action ActionOnServerConnect;
        public event Action<NetworkConnectionToClient> ActionOnServerDisconnectWithArg;
        public event Action ActionOnServerDisconnect;
        public event Action ActionOnServerSceneChanged;
        public event Action<string> ActionOnServerSceneChangedWithArg;
        
        public event Action ActionOnStartClient;
        public event Action ActionOnStopClient;
        public event Action ActionOnClientConnect;
        public event Action ActionOnClientDisconnect;

        public event Action ActionOnAnyChange;

        private Action<NetworkConnection> _cachedRemove;

        private int _ind;
        
        public override void Awake()
        {
            base.Awake();
            
            Cursor.lockState = CursorLockMode.Confined;

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
            GameObject mirrorPlayerStateSyncer = Instantiate(mirrorPlayerStateSyncerPrefab.gameObject);
            GameObject spawnPointSyncer = Instantiate(spawnPointSyncerPrefab.gameObject);
            GameObject gameStateManager = Instantiate(gameStateManagerPrefab.gameObject);
            GameObject killFeedSyncer = Instantiate(killFeedSyncerPrefab.gameObject);
            GameObject mirrorTimerSyncer = Instantiate(mirrorTimerSyncerPrefab.gameObject);
            
            NetworkServer.Spawn(mirrorPlayerStateSyncer);
            NetworkServer.Spawn(spawnPointSyncer);
            NetworkServer.Spawn(gameStateManager);
            NetworkServer.Spawn(killFeedSyncer);
            NetworkServer.Spawn(mirrorTimerSyncer);
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
        
        public override void OnStopHost()
        {
            base.OnStopHost();

            ActionOnHostStop?.Invoke();

            Debug.Log("[Server] OnStopHost");
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
            
            Connections.Remove(conn);

            SpawnedPlayerInstances.Remove(SpawnedPlayerInstances.FirstOrDefault(x => x.connectionToClient == conn));
            
            ActionOnServerDisconnect?.Invoke();
            ActionOnServerDisconnectWithArg?.Invoke(conn);

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


        private int _posInd;
        [Server]
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer, GameObject gamePlayer)
        {
            SpawnedPlayerInstances.Add(gamePlayer.GetComponent<MirrorPlayer>());

            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, ReplacePlayerOptions.Destroy);

            gamePlayer.name = conn.connectionId.ToString();

            // Vector3 pos = SpawnPointSyncer.Instance.GetFreePosition(_posInd++).Position;
            // Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(pos);
            // SpawnedPlayerInstances.Last().SetRigidbodyPositionAndRotation(pos, rot);
            // SpawnedPlayerInstances.Last().SetRagdollPositionAndRotation(pos, rot);
            
            OnPlayerLoadedOnLevel?.Invoke();
            OnPlayerLoadedOnLevelWithArg?.Invoke(conn);

            Debug.Log($"[Server] OnRoomServerSceneLoadedForPlayer {conn.address}");

            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }

        public override void ReadyStatusChanged()
        {
            base.ReadyStatusChanged();

            OnPlayerReadyChanged?.Invoke(Connections);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);

            ActionOnServerSceneChanged?.Invoke();
            ActionOnServerSceneChangedWithArg?.Invoke(sceneName);
        }

        #endregion

        #region Client

        [Client]
        public override void OnStartClient()
        {
            base.OnStartClient();

            ActionOnStartClient?.Invoke();

            Debug.Log($"[Client] OnStartClient: networkAddress:{networkAddress}");
        }
        
        public override void OnStopClient()
        {
            base.OnStopClient();

            ActionOnStopClient?.Invoke();

            Debug.Log($"OnStartClient: networkAddress:{networkAddress}");
        }

        [Client]
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            ActionOnClientConnect?.Invoke();

            Debug.Log($"[Client] OnClientConnect: networkAddress:{networkAddress}");
        }
        
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            ActionOnClientDisconnect?.Invoke();

            Debug.Log($"[Client] OnClientDisconnect: networkAddress:{networkAddress}");
        }

        [Client]
        public override void OnClientError(TransportError error, string reason)
        {
            base.OnClientError(error, reason);

            Debug.LogError($"[Client] OnClientError {error}, {reason}");
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