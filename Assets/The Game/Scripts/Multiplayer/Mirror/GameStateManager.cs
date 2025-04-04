using System.Linq;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.StateMachine;
using The_Game.Scripts.Main;
using UnityEngine;

namespace Shadow_Dominion
{
    public class GameStateManager : MirrorSingleton<GameStateManager>
    {
        [SerializeField] private LevelSO levelSo;
        
        private int _deadPlayers;
        private int _currentRound;

        private bool _isSubscribed;

        private new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            MirrorServer.Instance.ActionOnServerDisconnect += CheckWin;
            
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += Subscribe;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel += UpdateLevelListing;
            
            MirrorServer.Instance.ActionOnServerSceneChangedWithArg += SubscribeToLevelStateMachine;
            MirrorServer.Instance.ActionOnServerSceneChangedWithArg += UnSubscribeToLevelStateMachine;
        }

        private void OnDestroy()
        {
            MirrorServer.Instance.ActionOnServerDisconnect -= CheckWin;
            
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= InitializeLevel;
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= Subscribe;
            UnSubscribe();
            MirrorPlayersSyncer.Instance.OnAllPlayersLoadedOnLevel -= UpdateLevelListing;
            
            MirrorServer.Instance.ActionOnServerSceneChangedWithArg -= SubscribeToLevelStateMachine;
            MirrorServer.Instance.ActionOnServerSceneChangedWithArg -= UnSubscribeToLevelStateMachine;
        }

        private void SubscribeToLevelStateMachine(string sceneName)
        {
            if (SceneNamesStorage.GamePlayScene != sceneName)
                return;
            
            MirrorLevelSyncer.Instance.OnUpdate += RpcUpdateLevelState;

            _isSubscribed = true;
        }
        
        private void UnSubscribeToLevelStateMachine(string sceneName)
        {
            if (SceneNamesStorage.GamePlayScene == sceneName || !_isSubscribed)
                return;
            
            MirrorLevelSyncer.Instance.OnUpdate -= RpcUpdateLevelState;

            _isSubscribed = false;
        }

        #region Server
        
        [Server]
        private void InitializeLevel()
        {
            RpcUpdateLevelState(new LevelState(typeof(LevelInitializeState).ToString()));
        }
        
        [Server]
        private void CheckWin()
        {
            if (++_currentRound >= levelSo.Rounds)
            {
                RpcUpdateLevelState(new LevelState(typeof(FinishState).ToString()));
                return;
            }
            
            if (++_deadPlayers < MirrorPlayersSyncer.Instance.Players.Length - 1)
                return;

            RpcUpdateLevelState(new LevelState(typeof(LevelInitializeState).ToString()));

            _deadPlayers = 0;
        }
        
        [Server]
        private void Subscribe()
        {
            foreach (var player in MirrorServer.Instance.SpawnedPlayerInstances)
            {
                player.OnDead += CheckWin;
            }
        }
        
        private void UnSubscribe()
        {
            foreach (var player in MirrorServer.Instance.SpawnedPlayerInstances.Where(player => player))
            {
                player.OnDead -= CheckWin;
            }
        }

        [Server]
        private void UpdateLevelListing()
        {
            RpcUpdateLevelListing(MirrorPlayersSyncer.Instance.Players);
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcUpdateLevelState(LevelState levelState)
        {
            MirrorLevelSyncer.Instance.SetState(levelState);
        }
        
        [ClientRpc]
        private void RpcUpdateLevelListing(PlayerViewData[] players)
        {
            LevelPlayerListing.Instance.AddView(players);
        }

        #endregion
    }
}