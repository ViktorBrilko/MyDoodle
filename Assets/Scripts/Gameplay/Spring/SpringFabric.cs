using UnityEngine;
using Zenject;

public class SpringFabric : BaseFabric<Spring>
{
    private SpringConfig _springConfig;
    private GameObject _prefab;

    public SpringFabric(DiContainer container, SpringConfig springConfig, GameObject prefab) : base(container)
    {
        _springConfig = springConfig;
        _prefab = prefab;
    }

    public override Spring Create(Transform parent)
    {
        Spring spring = Container.InstantiatePrefabForComponent<Spring>(_prefab, parent,
            new object[] { _springConfig.JumpForce });

        return spring;
    }
}