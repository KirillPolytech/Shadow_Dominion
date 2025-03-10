﻿using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Shadow_Dominion
{
    public abstract class Pool<T> where T : Object
    {
        private readonly Factory<T> _factory;
        private readonly int _count;
        protected readonly List<T> _objects = new List<T>();

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

        public T Pull()
        {
            var firstOrDefault = _objects.FirstOrDefault(x => !x.GameObject().activeSelf);

            if (!firstOrDefault)
                firstOrDefault = Create();
            
            firstOrDefault.GameObject().SetActive(true);

            return firstOrDefault;
        }

        public void Push(T t)
        {
            t.GameObject().SetActive(false);
        }
    }
}