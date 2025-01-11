using Zenject;

namespace Shadow_Dominion
{
    public class PlayerPool : Pool<Shadow_Dominion.Main.Player>
    {
        [Inject]
        public PlayerPool(PlayerFactory factory, int count) : base(factory, count) { }
    }
}