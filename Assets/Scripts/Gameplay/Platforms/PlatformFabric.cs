using UnityEngine;
using Zenject;

public class PlatformFabric : BaseFabric<Platform>
{
    private PlatformConfig _platformConfig;
    private GameObject _prefab;

    public PlatformFabric(DiContainer container, PlatformConfig platformConfig, GameObject prefab) : base(container)
    {
        _platformConfig = platformConfig;
        _prefab = prefab;
    }

    public override Platform Create(Transform parent)
    {
        Platform platform = Container.InstantiatePrefabForComponent<Platform>(_prefab, parent,
            new object[] { _platformConfig.SpringPosition});

        return platform;
    }
}