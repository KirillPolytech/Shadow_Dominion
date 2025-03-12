using Mirror;
using The_Game.Scripts.Main;
using UnityEngine.SceneManagement;
using WindowsSystem;

namespace Shadow_Dominion
{
    public class MenuWindowsController : WindowsController
    {
        public new void Start()
        {
            base.Start();
            
            OpenWindow(Current);

            MirrorServer.Instance.ActionOnStartClient += OpenWindow<PlayerListingWindow>;
            MirrorServer.Instance.ActionOnHostStart += OpenWindow<PlayerListingWindow>;

            MirrorServer.Instance.ActionOnStopClient += OpenWindow<MainWindow>;
            MirrorServer.Instance.ActionOnHostStop += OpenWindow<MainWindow>;
            
            if (!NetworkServer.active && NetworkClient.isConnected && SceneManager.GetActiveScene().name == SceneNamesStorage.OnlineMenuScene)
            {
                OpenWindow<PlayerListingWindow>();
            }
        }

        protected void OnDestroy()
        {
            MirrorServer.Instance.ActionOnStartClient -= OpenWindow<PlayerListingWindow>;
            MirrorServer.Instance.ActionOnHostStart -= OpenWindow<PlayerListingWindow>;

            MirrorServer.Instance.ActionOnStopClient -= OpenWindow<MainWindow>;
            MirrorServer.Instance.ActionOnHostStop -= OpenWindow<MainWindow>;
        }
    }
}