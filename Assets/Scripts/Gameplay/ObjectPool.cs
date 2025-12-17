using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ObjectPool<T> : IInitializable where T : Component 
{
    private int _capacity;
    private IFabric<T> _fabric;
    private List<T> _pool = new();
    private Transform _container;

    public ObjectPool(IFabric<T> fabric, Transform container, int capacity)
    {
        _fabric = fabric;
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
            T spawned = _fabric.Create(_container.transform);
            spawned.gameObject.SetActive(false);

            _pool.Add(spawned);
        }
    }
}