using Shadow_Dominion.Settings;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class BootContext : MonoInstaller
    {
        [SerializeField] private MirrorServer server;
        [Space] [SerializeField] private MirrorPlayerInstaller playerPrefab;
        [Range(0, 4)] [SerializeField] private int count;
        [Space] [SerializeField] private Bullet bulletPrefab;
        [Range(0, 600)] [SerializeField] private int poolCount;
        
        public override void InstallBindings()
        {
            Container.BindInstance(server).AsSingle();
            
            Container.Bind<InputHandler>().AsTransient();

            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle();

            Container.Bind<BulletFactory>().AsSingle().WithArguments(bulletPrefab);
            Container.Bind<BulletPool>().AsSingle().WithArguments(poolCount);

            Container.Bind<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
            Container.Bind<PlayerPool>().AsSingle().WithArguments(count);
        }
    }
}