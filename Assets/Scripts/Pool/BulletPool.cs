using Zenject;

namespace HellBeavers
{
    public class BulletPool : Pool<Bullet>
    {
        [Inject]
        public BulletPool(BulletFactory bulletFactory, int count) : base(bulletFactory, count) { }
    }
}