using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
                //Debug.Log("StopHost.");
            }
            else if (NetworkServer.active)
            {
                _mirrorServer.StopServer();
                //Debug.Log("StopServer.");
            }
            else if (NetworkClient.isConnected)
            {
                _mirrorServer.StopClient();
                //Debug.Log("StopClient.");
            }
            else
            {
                Debug.Log("Not connected.");
            }
        });
    }
}