using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace HellBeavers
{
    public abstract class Pool<T> where T : Object
    {
        protected readonly Factory<T> _factory;
        protected readonly List<T> _objects = new List<T>();
        protected readonly int _count;

        public Pool(Factory<T> factory, int count)
        {
            _factory = factory;
            _count = count;
            
            for (int i = 0; i < _count; i++)
            {
                Create();
            }
        }

        private T Create()
        {
            T t = _factory.Create();
            _objects.Add(t);
            return t;
        }

        public T GetFree()
        {
            var firstOrDefault = _objects.FirstOrDefault(x => !x.GameObject().activeSelf);

            if (!firstOrDefault)
                firstOrDefault = Create();

            return firstOrDefault;
        }
    }
}