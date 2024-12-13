using HellBeavers;
using HellBeavers.Player;
using HellBeavers.Settings;
using UnityEngine;
using Zenject;

public class BootContext : MonoInstaller
{
    [SerializeField] private MirrorServer server;
    [SerializeField] private Lobby lobby;
    [Space]
    [SerializeField] private Player playerPrefab;
    [Range(0,4)][SerializeField] private int count;
    [Space]
    [SerializeField] private Bullet bulletPrefab;
    [Range(0,600)][SerializeField] private int poolCount;

    
    public override void InstallBindings()
    {
        Container.BindInstance(server).AsSingle();
        Container.BindInstance(lobby).AsSingle();
        
        Container.Bind<CursorService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();
        
        Container.Bind<BulletFactory>().AsSingle().WithArguments(bulletPrefab);
        Container.Bind<BulletPool>().AsSingle().WithArguments(poolCount);
        
        Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
        Container.Bind<PlayerPool>().AsSingle().WithArguments(count);
    }
}