using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace Shadow_Dominion
{
    public class MirrorPlayersSyncer : MirrorSingleton<MirrorPlayersSyncer>
    {
        private readonly SyncList<PlayerViewData> _players = new SyncList<PlayerViewData>();

        public event Action OnAllPlayersLoadedOnLevel;
        
        public PlayerViewData[] Players => _players.ToArray();
        
        [SyncVar]
        public int SpawnedPlayersOnLevel;

        #region Server
        
        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();

            _players.OnChange += OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnectWithArg += AddAddress;
            MirrorServer.Instance.ActionOnServerDisconnectWithArg += RemoveAddress;
            MirrorServer.Instance.OnPlayerReadyChanged += UpdateReadyState;
            MirrorServer.Instance.OnPlayerLoadedOnLevel += UpdateLoadedPlayersOnLevel;

            foreach (var networkConnection in MirrorServer.Instance.Connections)
            {
                _players.Add(new PlayerViewData(networkConnection.address, false));
            }
        }
        
        [Server]
        public override void OnStopServer()
        {
            _players.OnChange -= OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnectWithArg -= AddAddress;
            MirrorServer.Instance.ActionOnServerDisconnectWithArg -= RemoveAddress;
            MirrorServer.Instance.OnPlayerReadyChanged -= UpdateReadyState;
            MirrorServer.Instance.OnPlayerLoadedOnLevel -= UpdateLoadedPlayersOnLevel;

            SpawnedPlayersOnLevel = 0;
        }

        [Client]
        public override void OnStartClient()
        {
            UpdateViews();
        }

        [Server]
        private void AddAddress(NetworkConnectionToClient networkConnectionToClient)
        {
            _players.Add(new PlayerViewData(networkConnectionToClient.address, false));
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
            {
                UpdateReadyState(new PlayerViewData(networkRoomPlayer.connectionToClient.address, networkRoomPlayer.readyToBegin));
            }
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
        
        [Command(requiresAuthority = false)]
        public void CmdChangeState(NetworkRoomPlayer networkRoom)
        {
            networkRoom.CmdChangeReadyState(!networkRoom.readyToBegin);
        }
    }
}