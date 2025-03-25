using Mirror;

namespace Shadow_Dominion
{
    public class GameStateManager : MirrorSingleton<GameStateManager>
    {
        private int _deadPlayers = 0;
        
        private new void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
            
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += Subscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UnSubscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;
            
            MirrorServer.Instance.ActionOnClientDisconnect += UpdateLevelListing;
        }

        private void OnDestroy()
        {
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= Subscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= UnSubscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= UpdateLevelListing;
            
            MirrorServer.Instance.ActionOnClientDisconnect -= UpdateLevelListing;
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
            if (++_deadPlayers >= MirrorPlayersSyncer.Instance.SpawnedPlayersOnLevel)
            {
                SpawnPointSyncer.Instance.Reset();
            }
        }
        
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
    }
}