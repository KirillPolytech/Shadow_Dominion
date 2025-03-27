using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorPlayersSyncer : MirrorSingleton<MirrorPlayersSyncer>
    {
        private readonly SyncList<PlayerViewData> _playersViewData = new();

        public readonly List<NetworkConnectionToClient> Connections = new();

        public event Action OnAllPlayersLoadedOnLevel;

        public PlayerViewData[] Players => _playersViewData.ToArray();
        public PlayerViewData LocalPlayer => _playersViewData.First(x => UserData.Instance.Nickname == x.Nick);

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

            _playersViewData.OnChange += OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnect += UpdateConnections;
            MirrorServer.Instance.ActionOnServerDisconnect += UpdateConnections;
            MirrorServer.Instance.ActionOnServerConnect += UpdateViews;
            MirrorServer.Instance.ActionOnServerDisconnect += UpdateViews;
            MirrorServer.Instance.OnPlayerLoadedOnLevel += UpdateLoadedPlayersOnLevel;
        }

        [Server]
        private void UpdateConnections()
        {
            Connections.Clear();
            Connections.AddRange(MirrorServer.Instance.Connections);
        }

        [Server]
        public override void OnStopServer()
        {
            _playersViewData.OnChange -= OnSyncListChanged;

            MirrorServer.Instance.ActionOnServerConnect -= UpdateConnections;
            MirrorServer.Instance.ActionOnServerDisconnect -= UpdateConnections;
            MirrorServer.Instance.ActionOnServerConnect -= UpdateViews;
            MirrorServer.Instance.ActionOnServerDisconnect -= UpdateViews;
            MirrorServer.Instance.OnPlayerLoadedOnLevel -= UpdateLoadedPlayersOnLevel;
        }

        [Command(requiresAuthority = false)]
        public void UpdateReadyState(bool isReady, string nick)
        {
            PlayerViewData playerViewData = _playersViewData.First(x => x.Nick == nick);

            _playersViewData.Remove(playerViewData);

            playerViewData.IsReady = isReady;

            _playersViewData.Add(playerViewData);

            RpcUpdateReadyState(_playersViewData.ToArray());
            
            Debug.Log($"[Server] Player {nick} has been requested ready.");
        }

        [Server]
        private void UpdateLoadedPlayersOnLevel()
        {
            SpawnedPlayersOnLevel++;

            if (SpawnedPlayersOnLevel == _playersViewData.Count)
            {
                OnAllPlayersLoadedOnLevel?.Invoke();
            }
        }

        [Command(requiresAuthority = false)]
        private void AddToSyncList(PlayerViewData playerViewData)
        {
            _playersViewData.Add(playerViewData);
        }

        [Command(requiresAuthority = false)]
        private void RemoveFromSyncList(string nick)
        {
            PlayerViewData playerViewData = _playersViewData.FirstOrDefault(x => x.Nick == nick);

            if (playerViewData.Equals(default))
            {
                Debug.LogWarning($"Can't remove {nick}");
                return;
            }

            int ind = _playersViewData.IndexOf(playerViewData);
            _playersViewData.RemoveAt(ind);
        }

        public void DisconnectPlayer(int id)
        {
            NetworkConnectionToClient networkConnectionToClient =
                MirrorServer.Instance.Connections.FirstOrDefault(x => x.connectionId == id);

            networkConnectionToClient.Disconnect();
        }

        #endregion

        #region Client
        
        public override void OnStartClient()
        {
            _playersViewData.OnChange += OnSyncListChanged;

            AddToSyncList(
                new PlayerViewData(UserData.Instance.Nickname, false, 0));

            UpdateViews();
        }

        public override void OnStopClient()
        {
            _playersViewData.OnChange -= OnSyncListChanged;
        }

        [Client]
        private void OnSyncListChanged(SyncList<PlayerViewData>.Operation operation, int value, PlayerViewData str)
        {
            UpdateViews();
        }

        [Client]
        private void UpdateViews()
        {
            PlayerListing.Instance.SpawnView(_playersViewData.ToArray());
        }

        [Client]
        [ClientRpc]
        private void RpcUpdateReadyState(PlayerViewData[] playerViewData)
        {
            foreach (var viewData in playerViewData)
            {
                PlayerListing.Instance.IsReady(viewData.Nick, viewData.IsReady);
            }
        }

        #endregion
    }
}