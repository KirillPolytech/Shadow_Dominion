using Zenject;

namespace HellBeavers
{
    public class PlayerFactory : Factory<Player.Player>
    {
        [Inject]
        public PlayerFactory(IInstantiator instantiator, Player.Player prefab) : base(instantiator, prefab) { }
    }
}