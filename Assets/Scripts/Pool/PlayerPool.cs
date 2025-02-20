using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Shadow_Dominion
{
    public class PlayerPool : Pool<Main.Player>
    {
        [Inject]
        public PlayerPool(PlayerFactory factory, int count) : base(factory, count) { }

        public IEnumerable<Main.Player> GetActivePlayers() => _objects.Where(x => x.isActiveAndEnabled);
    }
}