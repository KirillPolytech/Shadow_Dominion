using Zenject;

namespace Shadow_Dominion
{
    public class PlayerFactory : Factory<MirrorPlayerInstaller>
    {
        [Inject]
        public PlayerFactory(IInstantiator instantiator, MirrorPlayerInstaller prefab) : base(instantiator, prefab) { }
    }
}