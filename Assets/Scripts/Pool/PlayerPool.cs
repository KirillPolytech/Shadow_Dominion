using Zenject;

namespace HellBeavers
{
    public class PlayerPool : Pool<Player.Player>
    {
        [Inject]
        public PlayerPool(PlayerFactory factory, int count) : base(factory, count) { }
    }
}