using UnityEngine;
using Zenject;

public class EnemyFabric : BaseFabric<Enemy>
{
    private EnemyConfig _enemyConfig;
    private GameObject _prefab;

    public EnemyFabric(DiContainer container, EnemyConfig enemyConfig, GameObject prefab) : base(container)
    {
        _enemyConfig = enemyConfig;
        _prefab = prefab;
    }

    public override Enemy Create(Transform parent)
    {
        Enemy enemy = Container.InstantiatePrefabForComponent<Enemy>(_prefab, parent,
            new object[] { _enemyConfig.Health, _enemyConfig.Score });

        return enemy;
    }
}