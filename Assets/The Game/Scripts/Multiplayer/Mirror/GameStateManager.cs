using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion
{
    public class GameStateManager : MirrorSingleton<GameStateManager>
    {
        private int _deadPlayers = 0;

        private new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += Subscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;

            MirrorServer.Instance.ActionOnClientDisconnect += UpdateLevelListing;
        }

        private void OnDestroy()
        {
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= Subscribe;
            UnSubscribe();
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= UpdateLevelListing;

            MirrorServer.Instance.ActionOnClientDisconnect -= UpdateLevelListing;
        }

        private void InitializeLevel()
        {
            UpdateLevelState();
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
            if (++_deadPlayers < MirrorPlayersSyncer.Instance.SpawnedPlayersOnLevel)
                return;

            SpawnPointSyncer.Instance.Reset();
            UpdateLevelState();
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
        private void UpdateLevelState()
        {
            MirrorLevelSyncer.Instance.CmdSetState(new LevelState(typeof(LevelInitializeState).ToString()));
        }

        #endregion
    }
}