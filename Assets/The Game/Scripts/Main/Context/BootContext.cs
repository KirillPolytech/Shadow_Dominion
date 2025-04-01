using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Settings;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        [SerializeField]
        private TextSO textSo;
        
        [FormerlySerializedAs("playerPrefab")]
        [Space]
        [SerializeField]
        private Main.MirrorPlayer mirrorPlayerPrefab;
        
        [Space]
        [SerializeField]
        private CoroutineExecuter coroutineExecuter;
        
        private UserData _userData;
        
        public override void InstallBindings()
        {
            _userData = new UserData();
            
            Container.BindInstance(roomSettings).AsSingle();
            Container.BindInstance(levelSO).AsSingle();
            Container.BindInstance(applicationSettingsSo).AsSingle();
            Container.BindInstance(textSo).AsSingle();
            
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();
            
            Container.BindInstance(coroutineExecuter).AsSingle();
        }
    }
}