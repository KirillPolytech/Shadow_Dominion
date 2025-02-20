using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class DisconnectButton : Button
    {
        private MirrorServer _mirrorServer;

        [Inject]
        public void Construct(MirrorServer mirrorServer)
        {
            _mirrorServer = mirrorServer;
        }

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(() =>
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    _mirrorServer.StopHost();
                    
                    Debug.LogWarning("StopHost.");
                }
                else if (NetworkServer.active)
                {
                    _mirrorServer.StopServer();

                    Debug.LogWarning("StopServer.");
                }
                else if (NetworkClient.isConnected)
                {
                    _mirrorServer.StopClient();
                    
                    Debug.LogWarning("StopClient.");
                }
                else
                {
                    Debug.LogWarning("Not connected.");
                }
                
                SceneManager.LoadScene("Menu");
            });
        }
    }
}