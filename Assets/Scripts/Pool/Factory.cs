using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public abstract class Factory<T> where T : Object
    {
        protected readonly T _prefab;
        protected readonly Transform _parent;
        protected readonly IInstantiator _instantiator;

        public Factory(IInstantiator instantiator, T prefab)
        {
            _instantiator = instantiator;
            _prefab = prefab;
            _parent = new GameObject($"{typeof(T)} parent").transform;
        }
        
        public virtual T Create()
        {
            T t = _instantiator.InstantiatePrefabForComponent<T>(_prefab);
            t.GameObject().transform.SetParent(_parent);
            t.name = t.GetType().ToString();
            return t;
        }
    }
}