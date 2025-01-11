using Zenject;

namespace Shadow_Dominion
{
    public class BulletPool : Pool<Bullet>
    {
        [Inject]
        public BulletPool(BulletFactory bulletFactory, int count) : base(bulletFactory, count) { }
    }
}