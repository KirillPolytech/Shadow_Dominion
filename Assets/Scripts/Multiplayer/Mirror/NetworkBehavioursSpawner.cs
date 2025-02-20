using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shadow_Dominion
{
    public class NetworkBehavioursSpawner
    {
        private readonly MirrorServer _mirrorServer;
        private readonly Action<NetworkConnection> _cached;
        private readonly LobbyNamesSyncer _lobbyNamesSyncer;
        private readonly NetworkBehaviour[] _networkBehaviours;
        private readonly NetworkRoomPlayer _networkRoomPlayer;

        public event Action OnPlayerReady;

        public NetworkBehavioursSpawner(
            MirrorServer mirrorServer,
            LobbyNamesSyncer lobbyNamesSyncer,
            CoroutineExecuter coroutineExecuter,
            NetworkBehavioursProvider networkBehaviours,
            NetworkRoomPlayer networkRoomPlayer)
        {
            _lobbyNamesSyncer = lobbyNamesSyncer;
            _mirrorServer = mirrorServer;
            _networkBehaviours = networkBehaviours.NetworkBehaviours;
            _networkRoomPlayer = networkRoomPlayer;

            _cached = arg => coroutineExecuter.Execute(WaitForClientReady(arg));
            _mirrorServer.ActionOnServerConnectWithArg += _cached;
        }

        ~NetworkBehavioursSpawner()
        {
            _mirrorServer.ActionOnServerConnectWithArg -= _cached;
        }

        private IEnumerator WaitForClientReady(NetworkConnection conn)
        {
            while (!conn.isReady)
            {
                yield return new WaitForFixedUpdate();
            }
            
            OnPlayerReady?.Invoke();

            Dispawn();
            Spawn(conn);
            UpdateListing();
        }

        private void UpdateListing()
        {
            string[] names = NetworkServer.connections
                .Select(x => x.Value.address).ToArray();
            _lobbyNamesSyncer.UpdateList(names);
        }

        private void Spawn(NetworkConnectionToClient conn)
        {
            foreach (var varNetworkBehaviour in _networkBehaviours)
            {
                NetworkServer.Spawn(varNetworkBehaviour.gameObject);
            }

            NetworkServer.AddPlayerForConnection(conn, Object.Instantiate(_networkRoomPlayer).gameObject);
        }

        private void Dispawn()
        {
            foreach (var varNetworkBehaviour in _networkBehaviours)
            {
                if (varNetworkBehaviour)
                    NetworkServer.UnSpawn(varNetworkBehaviour.gameObject);
            }
        }
    }
}