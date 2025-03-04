using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorPlayersSyncer : MirrorSingleton<MirrorPlayersSyncer>
    {
        private readonly SyncList<string> _players = new SyncList<string>();

        public event Action<List<string>> OnPlayersListChanged;
        public List<string> PlayersAddresses => _players.ToList();

        private MirrorServer _mirrorServer;

        [Inject]
        public void Construct(MirrorServer mirrorServer)
        {
            _mirrorServer = mirrorServer;
        }

        private void OnEnable()
        {
            _players.OnChange += OnSyncListChanged;
            
            if (!isServer)
                return;
            
            _mirrorServer.ActionOnServerConnectWithArg += AddAddress;
            _mirrorServer.ActionOnServerDisconnectWithArg += RemoveAddress;
        }

        private void OnDisable()
        {
            _players.OnChange -= OnSyncListChanged;
            
            if (!isServer)
                return;
            
            _mirrorServer.ActionOnServerConnectWithArg -= AddAddress;
            _mirrorServer.ActionOnServerDisconnectWithArg -= RemoveAddress;
        }

        private void AddAddress(NetworkConnectionToClient networkConnectionToClient)
        {
            _players.Add(networkConnectionToClient.address);
        }
        
        private void RemoveAddress(NetworkConnectionToClient networkConnectionToClient)
        {
            _players.Remove(networkConnectionToClient.address);
        }

        private void OnSyncListChanged(SyncList<string>.Operation operation, int value, string str)
        {
            OnPlayersListChanged?.Invoke(_players.ToList());
        }
    }
}