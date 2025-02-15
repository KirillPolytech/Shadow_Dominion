using Mirror;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class StartButton : Button
{
    private MirrorServer _mirrorServer;
    private bool _isInitialized;
    private UnityAction _action;

    [Inject]
    public void Construct(MirrorServer mirrorServer, RoomSettings roomSettings)
    {
        _mirrorServer = mirrorServer;

        _mirrorServer.ActionOnHostStart += Subscribe;
        _mirrorServer.ActionOnHostStop += Unsubscribe;

        _action = () => NetworkManager.singleton.ServerChangeScene(roomSettings.mainLevel);
        
        _isInitialized = true;
    }
    
    private void Subscribe() => onClick.AddListener(_action.Invoke);
    private void Unsubscribe() => onClick.RemoveListener(_action.Invoke);

    protected override void OnDisable()
    {
        base.OnDisable();
        
        if (!_isInitialized)
            return;
        
        _mirrorServer.ActionOnHostStart -= Subscribe;
        _mirrorServer.ActionOnHostStop -= Unsubscribe;
    }
}