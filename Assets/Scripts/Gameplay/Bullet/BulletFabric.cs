using UnityEngine;
using Zenject;

public class BulletFabric : BaseFabric<Bullet>
{
    private BulletConfig _bulletConfig;
    private GameObject _prefab;

    public BulletFabric(DiContainer container, BulletConfig bulletConfig, GameObject prefab) : base(container)
    {
        _bulletConfig = bulletConfig;
        _prefab = prefab;
    }

    public override Bullet Create(Transform parent)
    {
        Bullet bullet = Container.InstantiatePrefabForComponent<Bullet>(_prefab, parent,
            new object[] { _bulletConfig.Speed, _bulletConfig.Damage, _bulletConfig.MaxLifetime });

        return bullet;
    }
}