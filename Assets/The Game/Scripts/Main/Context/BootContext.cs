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
        private ApplicationSettingsSO applicationSettingsSo;
        
        [SerializeField]
        private RoomSettings roomSettings;
        
        [SerializeField]
        private LevelSO levelSO;
        
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
            Container.BindInstance(applicationSettingsSo).AsSingle();
            
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();
            
            Container.BindInstance(coroutineExecuter).AsSingle();
            
            Container.Bind<PositionMessage[]>().FromInstance(spawnPositions);
        }
    }
}