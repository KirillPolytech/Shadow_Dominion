using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Shadow_Dominion
{
    public class DisconnectButton : Button
    {
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(() =>
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    MirrorServer.Instance.StopHost();
                    
                    Debug.Log("StopHost.");
                }
                else if (NetworkServer.active)
                {
                    MirrorServer.Instance.StopServer();

                    Debug.Log("StopServer.");
                }
                else if (NetworkClient.isConnected)
                {
                    MirrorServer.Instance.StopClient();
                    
                    Debug.Log("StopClient.");
                }
                else
                {
                    Debug.Log("Not connected.");
                }
            });
        }
    }
}