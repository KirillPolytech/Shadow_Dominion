using Zenject;

namespace Shadow_Dominion
{
    public class PlayerFactory : Factory<Main.Player>
    {
        [Inject]
        public PlayerFactory(IInstantiator instantiator, Main.Player prefab) : base(instantiator, prefab) { }
    }
}