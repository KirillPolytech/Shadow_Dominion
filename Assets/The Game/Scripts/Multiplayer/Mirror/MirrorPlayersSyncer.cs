using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorPlayersSyncer : MirrorSingleton<MirrorPlayersSyncer>
    {
        private readonly SyncList<PlayerViewData> _players = new();

        public readonly List<NetworkConnectionToClient> Connections = new();

        public event Action OnAllPlayersLoadedOnLevel;

        public PlayerViewData[] Players => _players.ToArray();
        public PlayerViewData LocalPlayer => _players.First(x => x.IsLocalPlayer);

        [SyncVar] public int SpawnedPlayersOnLevel;

        private new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        #region Server

        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();

            _players.OnChange += OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnect += UpdateConnections;
            MirrorServer.Instance.ActionOnServerDisconnect += UpdateConnections;
            MirrorServer.Instance.ActionOnServerConnectWithArg += AddAddress;
            MirrorServer.Instance.ActionOnServerDisconnectWithArg += RemoveAddress;
            MirrorServer.Instance.OnPlayerReadyChanged += UpdateReadyState;
            MirrorServer.Instance.OnPlayerLoadedOnLevel += UpdateLoadedPlayersOnLevel;

            foreach (var networkConnection in MirrorServer.Instance.Connections)
                _players.Add(new PlayerViewData(
                        networkConnection.address, 
                        false, 
                        0, 
                        networkConnection.identity.isLocalPlayer));

            if (!_players.FirstOrDefault(x => x.Address == "localhost").Equals(default))
            {
                
            }
        }

        private void UpdateConnections()
        {
            Connections.Clear();
            Connections.AddRange(MirrorServer.Instance.Connections);
        }

        [Server]
        public override void OnStopServer()
        {
            _players.OnChange -= OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnect -= UpdateConnections;
            MirrorServer.Instance.ActionOnServerDisconnect -= UpdateConnections;
            MirrorServer.Instance.ActionOnServerConnectWithArg -= AddAddress;
            MirrorServer.Instance.ActionOnServerDisconnectWithArg -= RemoveAddress;
            MirrorServer.Instance.OnPlayerReadyChanged -= UpdateReadyState;
            MirrorServer.Instance.OnPlayerLoadedOnLevel -= UpdateLoadedPlayersOnLevel;
        }

        [Server]
        private void AddAddress(NetworkConnectionToClient networkConnectionToClient)
        {
            StartCoroutine(WaitForSpawn(networkConnectionToClient));
        }

        private IEnumerator WaitForSpawn(NetworkConnectionToClient networkConnectionToClient)
        {
            while (!networkConnectionToClient.identity)
            {
                yield return new WaitForFixedUpdate();
            }
            
            _players.Add(
                new PlayerViewData(
                    networkConnectionToClient.address, 
                    false, 
                    0, 
                    networkConnectionToClient.identity.isLocalPlayer)); 
        }

        [Server]
        private void RemoveAddress(NetworkConnectionToClient networkConnectionToClient)
        {
            int ind = _players.IndexOf(_players.FirstOrDefault(x => x.Address == networkConnectionToClient.address));
            _players.RemoveAt(ind);
        }

        [Server]
        private void UpdateReadyState(HashSet<NetworkRoomPlayer> networkConnectionToClient)
        {
            foreach (var networkRoomPlayer in networkConnectionToClient)
                UpdateReadyState(
                    new PlayerViewData(
                        networkRoomPlayer.connectionToClient.address, 
                        networkRoomPlayer.readyToBegin, 
                        0,
                        networkRoomPlayer.isLocalPlayer));
        }

        [Server]
        [ClientRpc]
        private void UpdateReadyState(PlayerViewData playerViewData)
        {
            PlayerListing.Instance.IsReady(playerViewData.Address, playerViewData.IsReady);
        }

        [Server]
        private void UpdateLoadedPlayersOnLevel()
        {
            SpawnedPlayersOnLevel++;

            if (SpawnedPlayersOnLevel == _players.Count)
            {
                OnAllPlayersLoadedOnLevel?.Invoke();
            }
        }

        #endregion

        #region Client

        [Client]
        public override void OnStartClient()
        {
            _players.OnChange += OnSyncListChanged;

            UpdateViews();
        }

        [Client]
        private void OnSyncListChanged(SyncList<PlayerViewData>.Operation operation, int value, PlayerViewData str)
        {
            UpdateViews();
        }

        [Client]
        private void UpdateViews()
        {
            PlayerListing.Instance.SpawnView(_players.ToArray());
        }

        #endregion
    }
}