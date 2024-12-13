using HellBeavers.Settings;
using UnityEngine;
using Zenject;

public class BootContext : MonoInstaller
{
    [SerializeField] private MirrorServer server;
    [SerializeField] private Lobby lobby;
    
    public override void InstallBindings()
    {
        Container.BindInstance(server).AsSingle();
        Container.BindInstance(lobby).AsSingle();
        
        Container.Bind<CursorService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();
    }
}