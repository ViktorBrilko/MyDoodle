using UnityEngine;
using Zenject;

public class EnemyFabric : BaseFabric<Enemy>
{
    private EnemyConfig _enemyConfig;

    public EnemyFabric(DiContainer container, EnemyConfig enemyConfig) : base(container)
    {
        _enemyConfig = enemyConfig;
    }

    public override Enemy Create(Transform parent)
    {
        Enemy enemy = Container.InstantiatePrefabForComponent<Enemy>(_enemyConfig.Prefab, parent,
            new object[] { _enemyConfig.Health });

        return enemy;
    }
}