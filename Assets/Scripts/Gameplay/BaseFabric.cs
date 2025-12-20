using UnityEngine;
using Zenject;

public abstract class BaseFabric<TProduct>
{
    protected readonly DiContainer Container;

    protected BaseFabric( DiContainer container)
    {
        Container = container;
    }

    public abstract TProduct Create(Transform parent);
}