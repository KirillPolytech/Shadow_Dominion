using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class GameStateManager : MirrorSingleton<GameStateManager>
    {
        [SerializeField] private LevelSO levelSo;
        
        private int _deadPlayers;
        private int _currentRound;

        private new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += Subscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;
        }

        private void OnDestroy()
        {
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= Subscribe;
            UnSubscribe();
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= UpdateLevelListing;
        }

        private void InitializeLevel()
        {
            UpdateLevelState(new LevelState(typeof(LevelInitializeState).ToString()));
        }

        private void Subscribe()
        {
            foreach (var player in MirrorServer.Instance.SpawnedPlayerInstances)
            {
                player.OnDead += CheckWin;
            }
        }

        private void UnSubscribe()
        {
            foreach (var player in MirrorServer.Instance.SpawnedPlayerInstances)
            {
                player.OnDead -= CheckWin;
            }
        }

        private void CheckWin()
        {
            if (++_currentRound >= levelSo.Rounds)
            {
                MirrorLevelSyncer.Instance.CmdSetState(new LevelState(typeof(FinishState).ToString()));
                return;
            }
            
            if (++_deadPlayers < MirrorPlayersSyncer.Instance.SpawnedPlayersOnLevel)
                return;

            SpawnPointSyncer.Instance.Reset();
            UpdateLevelState(new LevelState(typeof(LevelInitializeState).ToString()));
        }

        #region Server

        [Server]
        private void UpdateLevelListing()
        {
            RpcUpdateLevelListing(MirrorPlayersSyncer.Instance.Players);
        }

        [Server, ClientRpc]
        private void RpcUpdateLevelListing(PlayerViewData[] players)
        {
            LevelPlayerListing.Instance.AddView(players);
        }

        #endregion

        #region Client

        [ClientRpc]
        private void UpdateLevelState(LevelState levelState)
        {
            MirrorLevelSyncer.Instance.CmdSetState(levelState);
        }

        #endregion
    }
}