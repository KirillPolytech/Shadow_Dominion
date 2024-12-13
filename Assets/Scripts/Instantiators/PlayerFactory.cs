using UnityEngine;
using Zenject;

namespace HellBeavers
{
    public class PlayerFactory : Factory<GameObject>
    {
        public PlayerFactory(IInstantiator instantiator, GameObject prefab) : base(instantiator, prefab) { }
    }
}