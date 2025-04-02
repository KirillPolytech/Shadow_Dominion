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
        private readonly SyncList<PlayerViewData> _playersViewData = new();

        public readonly List<NetworkConnectionToClient> Connections = new();

        public event Action OnAllPlayersLoadedOnLevel;
        public PlayerViewData[] Players => _playersViewData.ToArray();
        public PlayerViewData LocalPlayer => _playersViewData.First(x => UserData.Instance.Nickname == x.Nick);

        private int _spawnedPlayersOnLevel;

        [SyncVar] private int _playerid;

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
            MirrorServer.Instance.OnPlayerLoadedOnLevelWithArg += UpdateLoadedPlayersOnLevel;
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
            MirrorServer.Instance.OnPlayerLoadedOnLevelWithArg -= UpdateLoadedPlayersOnLevel;
        }

        [Command(requiresAuthority = false)]
        public void UpdateReadyState(bool isReady, string nick)
        {
            PlayerViewData playerViewData = _playersViewData.First(x => x.Nick == nick);

            _playersViewData.Remove(playerViewData);

            playerViewData.IsReady = isReady;

            _playersViewData.Add(playerViewData);
            
            Debug.Log($"[Server] Player {nick} has been requested {isReady}. {Time.time}");
        }

        [Server]
        private void UpdateLoadedPlayersOnLevel(NetworkConnectionToClient networkConnectionToClient)
        {
            StartCoroutine(WaitForInitialize(networkConnectionToClient));
        }

        private IEnumerator WaitForInitialize(NetworkConnectionToClient networkConnectionToClient)
        {
            while (networkConnectionToClient.identity.netId == 0)
            {
                yield return new WaitForFixedUpdate();
            }
            
            if (++_spawnedPlayersOnLevel == _playersViewData.Count)
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
        public void UpdateView(string killerName)
        {
            PlayerViewData playerViewData = _playersViewData.First(x => x.Nick == killerName);
            _playersViewData.Remove(playerViewData);
            playerViewData.Kills += 1;
            
            _playersViewData.Add(playerViewData);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            _playersViewData.OnChange += OnSyncListChanged;

            AddToSyncList(
                new PlayerViewData(_playerid++ , UserData.Instance.Nickname, false, 0));

            UpdateViews();
        }

        public override void OnStopClient()
        {
            _playersViewData.OnChange -= OnSyncListChanged;
        }
        
        private void OnSyncListChanged(SyncList<PlayerViewData>.Operation operation, int value, PlayerViewData str)
        {
            UpdateViews();
        }
        
        private void UpdateViews()
        {
            PlayerListing.Instance.SpawnView(_playersViewData.ToArray());
            LevelPlayerListing.Instance?.AddView(_playersViewData.ToArray());
        }

        #endregion
    }
}