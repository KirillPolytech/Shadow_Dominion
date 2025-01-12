using Shadow_Dominion.Server;
using TMPro;
using UnityEngine;
using Zenject;

public class MenuContext : MonoInstaller
{
    [SerializeField] private MenuWindowsController menuWindowsController;
    [SerializeField] private RoomSettings roomSettings;
    [Space]
    [SerializeField] private TMP_InputField IP;
    [SerializeField] private TMP_InputField PORT;
    
    
    public override void InstallBindings()
    {
        IP.text = "127.0.0.1";
        PORT.text = "7777";
        
        Container.BindInstance(menuWindowsController).AsSingle();
        Container.BindInstance(roomSettings).AsSingle();
        
        Container.Bind<RoomFactory>().AsSingle();
        Container.Bind<LobbyFactory>().AsSingle();
        Container.Bind<IPInputFieldProvider>().AsSingle().WithArguments(IP);
        Container.Bind<PORTInputFieldProvider>().AsSingle().WithArguments(PORT);
    }
}