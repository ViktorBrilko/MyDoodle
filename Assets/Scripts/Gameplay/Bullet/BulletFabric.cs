using UnityEngine;
using Zenject;

public class BulletFabric : BaseFabric<Bullet>
{
    private BulletConfig _bulletConfig;

    public BulletFabric(DiContainer container, BulletConfig bulletConfig) : base(container)
    {
        _bulletConfig = bulletConfig;
    }

    public override Bullet Create(Transform parent)
    {
        Bullet bullet = Container.InstantiatePrefabForComponent<Bullet>(_bulletConfig.Prefab, parent,
            new object[] {  _bulletConfig.Damage, _bulletConfig.Speed, _bulletConfig.MaxLifetime });

        return bullet;
    }
}