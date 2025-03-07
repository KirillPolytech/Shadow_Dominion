using Mirror;
using The_Game.Scripts.Main;
using UnityEngine.SceneManagement;
using WindowsSystem;
using Zenject;

namespace Shadow_Dominion
{
    public class MenuWindowsController : WindowsController
    {
        private MirrorServer _mirrorServer;

        [Inject]
        public void Construct(MirrorServer mirrorServer)
        {
            _mirrorServer = mirrorServer;
        }
        
        public new void Start()
        {
            base.Start();
            
            OpenWindow(Current);

            _mirrorServer.ActionOnStartClient += OpenWindow<PlayerListingWindow>;
            _mirrorServer.ActionOnHostStart += OpenWindow<PlayerListingWindow>;

            _mirrorServer.ActionOnStopClient += OpenWindow<MainWindow>;
            _mirrorServer.ActionOnHostStop += OpenWindow<MainWindow>;
            
            if (!NetworkServer.active && NetworkClient.isConnected && SceneManager.GetActiveScene().name == SceneNamesStorage.OnlineMenuScene)
            {
                OpenWindow<PlayerListingWindow>();
            }
        }

        protected void OnDestroy()
        {
            _mirrorServer.ActionOnStartClient -= OpenWindow<PlayerListingWindow>;
            _mirrorServer.ActionOnHostStart -= OpenWindow<PlayerListingWindow>;

            _mirrorServer.ActionOnStopClient -= OpenWindow<MainWindow>;
            _mirrorServer.ActionOnHostStop -= OpenWindow<MainWindow>;
        }
    }
}