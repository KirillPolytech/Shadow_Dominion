using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Settings;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class BootContext : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField]
        private RoomSettings roomSettings;
        
        [SerializeField]
        private LevelSO levelSO;

        [Space]
        [SerializeField]
        private MirrorServer mirrorServer;

        [Space]
        [SerializeField]
        private Main.Player playerPrefab;
        
        [SerializeField]
        private NetworkRoomPlayer networkRoomPlayerPrefab;

        [Space]
        [SerializeField]
        private CoroutineExecuter coroutineExecuter;

        [SerializeField]
        private PositionMessage[] spawnPositions;
        
        public override void InstallBindings()
        {
            Container.BindInstance(roomSettings).AsSingle();
            Container.BindInstance(levelSO).AsSingle();
            
            Container.BindInstance(mirrorServer).AsSingle();

            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();

            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
            Container.Bind<RoomPlayerFactory>().AsSingle().WithArguments(networkRoomPlayerPrefab);

            Container.BindInstance(coroutineExecuter).AsSingle();

            Container.Bind<MirrorPlayerSpawner>().AsSingle().NonLazy();
            
            Container.Bind<PositionMessage[]>().FromInstance(spawnPositions);
        }
    }
}