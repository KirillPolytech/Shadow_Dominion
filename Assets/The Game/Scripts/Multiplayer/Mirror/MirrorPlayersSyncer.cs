using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace Shadow_Dominion
{
    public class MirrorPlayersSyncer : MirrorSingleton<MirrorPlayersSyncer>
    {
        private readonly SyncList<PlayerViewData> _players = new SyncList<PlayerViewData>();

        #region Server
        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();

            _players.OnChange += OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnectWithArg += AddAddress;
            MirrorServer.Instance.ActionOnServerDisconnectWithArg += RemoveAddress;
            MirrorServer.Instance.OnPlayerReadyChanged += UpdateReadyState;

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
    }
}