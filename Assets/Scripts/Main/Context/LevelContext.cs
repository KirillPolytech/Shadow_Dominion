using Zenject;

namespace HellBeavers
{
    public class LevelContext : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ButtonsFactory>().AsSingle();
            Container.Bind<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputHandler>().AsTransient();
            Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle().NonLazy();
        }
    }
}