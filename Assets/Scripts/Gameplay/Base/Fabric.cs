using Gameplay;
using UnityEngine;
using Zenject;

public class Fabric<TProduct, TConfig> :  IFabric<TProduct>
{
    private DiContainer _container;
    private GameObject _prefab;
    private TConfig _config;

    [Inject]
    public Fabric(DiContainer container, GameObject prefab, TConfig config)
    {
        _container = container;
        _prefab = prefab;
        _config = config;
    }


    public TProduct Create(Transform parent)
    {
        TProduct product = _container.InstantiatePrefabForComponent<TProduct>(_prefab, parent,
            new object[] { _config });

        return product;
    }
}