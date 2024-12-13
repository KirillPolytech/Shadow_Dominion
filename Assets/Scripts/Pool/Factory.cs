using UnityEngine;
using Zenject;

namespace HellBeavers
{
    public abstract class Factory<T> where T : Object
    {
        protected readonly T _prefab;
        protected readonly GameObject _parent;
        protected readonly IInstantiator _instantiator;

        public Factory(IInstantiator instantiator, T prefab)
        {
            _instantiator = instantiator;
            _prefab = prefab;
            _parent = new GameObject($"{typeof(T)} parent");
        }
        
        public T Create()
        {
            T t = _instantiator.InstantiatePrefabForComponent<T>(_prefab, _parent);
            t.name = t.GetType().ToString();
            return t;
        }
    }
}