using UnityEngine;
using Zenject;

public class PlatformFabric : BaseFabric<Platform>
{
    private PlatformConfig _platformConfig;

    public PlatformFabric(DiContainer container, PlatformConfig platformConfig) : base(container)
    {
        _platformConfig = platformConfig;
    }

    public override Platform Create(Transform parent)
    {
        Platform platform = Container.InstantiatePrefabForComponent<Platform>(_platformConfig.Prefab, parent,
            new object[] { _platformConfig.SpringPosition});

        return platform;
    }
}