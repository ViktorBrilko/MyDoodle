using UnityEngine;
using Zenject;

public class BulletFabric : IFabric<Bullet>
{
    private int _damage;
    private int _speed;
    private float _maxLifetime;
    private GameObject _bulletPrefab;
    private readonly DiContainer _container;

    public BulletFabric(DiContainer container, BulletConfig bulletConfig, GameObject bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
        _container = container;
        _damage = bulletConfig.Damage;
        _speed = bulletConfig.Speed;
        _maxLifetime = bulletConfig.MaxLifetime;
    }

    public Bullet Create(Transform parent)
    {
        Bullet bullet = _container.InstantiatePrefabForComponent<Bullet>(_bulletPrefab, parent,
            new object[] { _damage, _speed, _maxLifetime });

        return bullet;
    }
}