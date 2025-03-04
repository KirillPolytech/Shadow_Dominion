using System;
using System.Collections;
using Mirror;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorLobby : MirrorSingleton<MirrorLobby>
    {
        public event Action<string> OnPlayerReadyWithAddress;
        public event Action OnPlayerReady;

        private Action<NetworkConnectionToClient> _cachedOnServerConnectWithArg;
        private MirrorServer _mirrorServer;
        private CoroutineExecuter _coroutineExecuter;

        [Inject]
        public void Construct(
            MirrorServer mirrorServer,
            CoroutineExecuter coroutineExecuter)
        {
            _mirrorServer = mirrorServer;
            _coroutineExecuter = coroutineExecuter;

            _cachedOnServerConnectWithArg = arg => _coroutineExecuter.Execute(WaitForClientReady(arg));
            
            _mirrorServer.ActionOnServerConnectWithArg += _cachedOnServerConnectWithArg;
            
            Debug.Log("Lobby spawned");
        }

        private void OnDestroy()
        {
            if (!isServer)
                return;
            
            _mirrorServer.ActionOnServerConnectWithArg -= _cachedOnServerConnectWithArg;
        }

        private IEnumerator WaitForClientReady(NetworkConnectionToClient conn)
        {
            while (!conn.isReady)
            {
                yield return new WaitForFixedUpdate();
            }
            
            OnPlayerReadyWithAddress?.Invoke(conn.address);
            OnPlayerReady?.Invoke();
            
            Debug.Log(isServer + " " + isOwned + " " + isClient + " " + netId + " " + conn.identity);
        }

        [ClientRpc]
        public void RpcSpawnView(string address)
        {
            if (!CheckPlayerListingInstance())
                return;
            
            PlayerListing.Instance.SpawnView(address);
        }
        
        [ClientRpc]
        public void RpcDispawnView(string address)
        {
            if (!CheckPlayerListingInstance())
                return;
            
            PlayerListing.Instance.Dispawn(address);
        }
            
        private bool CheckPlayerListingInstance()
        {
            if (!PlayerListing.Instance)
                Debug.LogWarning($"PlayerListing.Instance is null");
            
            return PlayerListing.Instance;
        }
    }
}

/*
private void SpawnRoomPlayer(NetworkConnectionToClient conn)
{
//GameObject instance = Instantiate(_networkRoomPlayerprefab).gameObject;
//_instances.Add(conn, instance);
// NetworkServer.AddPlayerForConnection(conn, instance.gameObject);
}

private void DispawnRoomPlayer(NetworkConnectionToClient conn)
{
//_instances.TryGetValue(conn, out GameObject value);

//if (!value)
//return;

//_instances.Remove(conn);
//NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Destroy);
}
*/