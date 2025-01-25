using Zenject;

namespace Shadow_Dominion
{
    public class PlayerPool : Pool<MirrorPlayerInstaller>
    {
        [Inject]
        public PlayerPool(PlayerFactory factory, int count) : base(factory, count) { }
    }
}