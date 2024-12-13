using Zenject;

namespace HellBeavers
{
    public class BulletFactory : Factory<Bullet>
    {
        [Inject]
        public BulletFactory(IInstantiator instantiator, Bullet prefab) : base(instantiator, prefab) { }
    }
}