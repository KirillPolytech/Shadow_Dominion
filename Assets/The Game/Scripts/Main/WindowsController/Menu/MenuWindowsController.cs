using Mirror;
using The_Game.Scripts.Main;
using UnityEngine.SceneManagement;
using WindowsSystem;

namespace Shadow_Dominion
{
    public class MenuWindowsController : WindowsController
    {
        protected new void Start()
        {
            base.Start();
            
            if (SceneManager.GetActiveScene().name == SceneNamesStorage.OnlineMenuScene)
            {
                MirrorServer.Instance.ActionOnStartClient += OpenWindow<PlayerListingWindow>;
                MirrorServer.Instance.ActionOnHostStart += OpenWindow<PlayerListingWindow>;
                
                OpenWindow<PlayerListingWindow>();
            }

            MirrorServer.Instance.ActionOnStopClient += OpenWindow<MainWindow>;
            MirrorServer.Instance.ActionOnHostStop += OpenWindow<MainWindow>;
        }

        protected void OnDestroy()
        {
            if (SceneManager.GetActiveScene().name == SceneNamesStorage.OnlineMenuScene)
            {
                MirrorServer.Instance.ActionOnStartClient -= OpenWindow<PlayerListingWindow>;
                MirrorServer.Instance.ActionOnHostStart -= OpenWindow<PlayerListingWindow>;
            }

            MirrorServer.Instance.ActionOnStopClient -= OpenWindow<MainWindow>;
            MirrorServer.Instance.ActionOnHostStop -= OpenWindow<MainWindow>;
        }
    }
}