using Unity.VisualScripting;
using Zenject;

namespace Shadow_Dominion
{
    public class PlayerFactory : Factory<Main.Player>
    {
        [Inject]
        public PlayerFactory(IInstantiator instantiator, Main.Player prefab) : base(instantiator, prefab) { }
        
        public override Main.Player Create()
        {
            Main.Player t = _instantiator.InstantiatePrefabForComponent<Main.Player>(_prefab);
            t.GameObject().transform.SetParent(_parent);
            t.name = t.GetType().ToString();
            return t;
        }
    }
}