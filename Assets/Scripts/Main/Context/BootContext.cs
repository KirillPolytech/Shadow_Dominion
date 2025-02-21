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
        [SerializeField]
        private MirrorServer mirrorServer;

        [Space]
        [SerializeField]
        private Main.Player playerPrefab;

        [Space]
        [SerializeField]
        private CoroutineExecuter coroutineExecuter;

        [SerializeField]
        private PositionMessage[] spawnPositions;

        [SerializeField]
        private NetworkRoomPlayer networkRoomPlayers;
        
        public override void InstallBindings()
        {
            Container.BindInstance(mirrorServer).AsSingle();
            Container.BindInstance(roomSettings).AsSingle();

            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();

            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);

            Container.BindInstance(coroutineExecuter).AsSingle();

            Container.Bind<MirrorPlayerSpawner>().AsSingle().NonLazy();
            
            Container.BindInstance(networkRoomPlayers).AsSingle();

            Container.Bind<PositionMessage[]>().FromInstance(spawnPositions);
        }
    }
}