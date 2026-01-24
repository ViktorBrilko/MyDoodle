using System;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [Serializable]
    public class Spawner<T> : IInitializable, IDisposable
        where T : Component, IResetable
    {
        private ObjectPool<T> _pool;
        private SignalBus _signalBus;

        public Spawner(ObjectPool<T> pool, SignalBus signalBus) {
           
            _pool = pool;
            _signalBus = signalBus;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ResetSignal<T>>(OnItemDestroy);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ResetSignal<T>>(OnItemDestroy);
        }

        public T SpawnItem(Vector3 spawnPoint)
        {
            T newItem;

            if (_pool.TryGetObject(out T item))
            {
                newItem = item;
            }
            else
            {
                newItem = _pool.AddNewItem();
            }

            SetItem(newItem, spawnPoint);
            
            return newItem;
        }

        private void SetItem(T item, Vector3 spawnPoint)
        {
            item.gameObject.SetActive(true);
            item.transform.position = spawnPoint;
        }

        private void OnItemDestroy(ResetSignal<T> signal)
        {
            T item = signal.Resetable;
            item.gameObject.SetActive(false);
            item.transform.SetParent(_pool.Container.transform);
            item.Reset();
        }
    }
}