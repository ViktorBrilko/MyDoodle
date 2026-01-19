using UnityEngine;
using Zenject;

public class SpringFabric : BaseFabric<Spring>
{
    private SpringConfig _springConfig;
    
    public SpringFabric(DiContainer container, SpringConfig springConfig) : base(container)
    {
        _springConfig = springConfig;
    }

    public override Spring Create(Transform parent)
    {
        Spring spring = Container.InstantiatePrefabForComponent<Spring>(_springConfig.Prefab, parent,
            new object[] { _springConfig.JumpForce});

        return spring;
    }
}