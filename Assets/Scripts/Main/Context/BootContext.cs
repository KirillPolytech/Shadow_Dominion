using Mirror;
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
        [Space]
        
        [SerializeField] private MirrorServer mirrorServer;
        [Space] [SerializeField] private Main.Player playerPrefab;
        [Range(0, 4)] [SerializeField] private int count;
        
        [Space]
        [SerializeField] private CoroutineExecuter coroutineExecuter;
        [SerializeField] private LobbyNamesSyncer lobbyNamesSyncer;
        [SerializeField] private PositionMessage[] spawnPositions;
        [SerializeField] private NetworkBehaviour[] networkBehaviours;
        [SerializeField] private NetworkRoomPlayer networkRoomPlayerPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInstance(mirrorServer).AsSingle();
            Container.BindInstance(roomSettings).AsSingle();
            
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();
            
            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
            Container.Bind<PlayerPool>().AsSingle().WithArguments(count);

            Container.BindInstance(coroutineExecuter).AsSingle();
            Container.BindInstance(lobbyNamesSyncer).AsSingle().NonLazy();
            Container.Bind<NetworkBehavioursSpawner>()
                .AsSingle()
                .WithArguments(coroutineExecuter, new NetworkBehavioursProvider(networkBehaviours), networkRoomPlayerPrefab).NonLazy();
            Container.Bind<MirrorPlayerSpawner>().AsSingle().WithArguments(spawnPositions).NonLazy();
        }
    }
}