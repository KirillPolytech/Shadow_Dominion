namespace HellBeavers
{
    public class PlayerPool : Pool<Player>
    {
        public PlayerPool(Factory<Player> factory, int count) : base(factory, count) { }
    }
}