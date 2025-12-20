using System;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class BaseSpawner<T> : IInitializable, IDisposable
        where T : Component, IResetable
    {
        private ObjectPool<T> _pool;
        private SignalBus _signalBus;

        public BaseSpawner(ObjectPool<T> pool, SignalBus signalBus)
        {
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

        public void SpawnItem(Transform spawnPoint)
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
        }

        private void SetItem(T item, Transform spawnPoint)
        {
            item.gameObject.SetActive(true);
            item.transform.position = spawnPoint.position;
        }

        private void OnItemDestroy(ResetSignal<T> signal)
        {
            T item = signal.Resetable;
            item.gameObject.SetActive(false);
            item.Reset();
        }
    }
}