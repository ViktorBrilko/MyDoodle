using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ObjectPool<T> : IInitializable where T : Component
    {
        private int _capacity;
        private IFactory<T> _factory;
        private List<T> _pool = new();
        private Transform _container;

        public Transform Container => _container;

        public ObjectPool(IFactory<T> factory, Transform container, int capacity)
        {
            _factory = factory;
            _capacity = capacity;
            _container = container;
        }

        public bool TryGetObject(out T result)
        {
            result = _pool.FirstOrDefault(p => p.gameObject.activeSelf == false);
            return result != null;
        }

        public void Initialize()
        {
            for (int i = 0; i < _capacity; i++)
            {
                AddNewItem();
            }
        }

        public T AddNewItem()
        {
            T spawned = _factory.Create(_container.transform);
            spawned.gameObject.SetActive(false);

            _pool.Add(spawned);
            return spawned;
        }
    }
}