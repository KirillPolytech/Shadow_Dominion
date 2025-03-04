using UnityEngine;
using WindowsSystem;
using Zenject;

namespace Shadow_Dominion
{
    public class LevelContext : MonoInstaller
    {
        [SerializeField] private WindowsController windowsController;
        
        public override void InstallBindings()
        {
            Container.Bind<ButtonsFactory>().AsSingle();
            Container.BindInstance(windowsController).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle().NonLazy();
        }
    }
}