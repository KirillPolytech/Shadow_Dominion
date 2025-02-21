using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorLobby : MirrorSingleton<MirrorLobby>
    {
        private readonly Dictionary<NetworkConnectionToClient, GameObject> _instances =
            new Dictionary<NetworkConnectionToClient, GameObject>();

        public event Action OnPlayerReady;
        public event Action<string> OnPlayerReadyWithAddress;
        public event Action<string> OnViewSpawn;

        private Action<NetworkConnectionToClient> _cached;
        private MirrorServer _mirrorServer;
        private CoroutineExecuter _coroutineExecuter;
        private NetworkRoomPlayer _networkRoomPlayerprefab;

        [Inject]
        public void Construct(
            MirrorServer mirrorServer,
            CoroutineExecuter coroutineExecuter,
            NetworkRoomPlayer networkRoomPlayerprefab)
        {
            _mirrorServer = mirrorServer;
            _coroutineExecuter = coroutineExecuter;
            _networkRoomPlayerprefab = networkRoomPlayerprefab;

            _cached = arg => _coroutineExecuter.Execute(WaitForClientReady(arg));

            _mirrorServer.ActionOnServerConnectWithArg += _cached;
            _mirrorServer.ActionOnServerDisconnectWithArg += DispawnRoomPlayer;
            OnPlayerReadyWithAddress += RpcSpawnView;
        }

        private void OnDestroy()
        {
            _mirrorServer.ActionOnServerConnectWithArg -= _cached;
            _mirrorServer.ActionOnServerDisconnectWithArg -= DispawnRoomPlayer;
            OnPlayerReadyWithAddress -= RpcSpawnView;
        }

        private IEnumerator WaitForClientReady(NetworkConnectionToClient conn)
        {
            while (!conn.isReady)
            {
                yield return new WaitForFixedUpdate();
            }

            SpawnRoomPlayer(conn);

            OnPlayerReady?.Invoke();
            OnPlayerReadyWithAddress?.Invoke(conn.address);
        }

        [ClientRpc]
        private void RpcSpawnView(string address)
        {
            OnViewSpawn?.Invoke(address);
        }

        private void SpawnRoomPlayer(NetworkConnectionToClient conn)
        {
            GameObject instance = Instantiate(_networkRoomPlayerprefab).gameObject;
            _instances.Add(conn, instance);
            NetworkServer.AddPlayerForConnection(conn, instance.gameObject);
        }

        private void DispawnRoomPlayer(NetworkConnectionToClient conn)
        {
            _instances.TryGetValue(conn, out GameObject value);

            if (!value)
                return;

            _instances.Remove(conn);
            NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Destroy);
        }
    }
}