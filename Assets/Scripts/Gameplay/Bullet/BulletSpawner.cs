using System;
using UnityEngine;
using Zenject;

public class BulletSpawner : IInitializable, IDisposable
{
    private ObjectPool<Bullet> _pool;
    private SignalBus _signalBus;

    public BulletSpawner(ObjectPool<Bullet> pool, SignalBus signalBus)
    {
        _pool = pool;
        _signalBus = signalBus;
    }

    public void Dispose()
    {
        Debug.Log("привет");
        _signalBus.Unsubscribe<ResetSignal<Bullet>>(OnBulletDestroy);
    }

    public void Initialize()
    {
        _signalBus.Subscribe<ResetSignal<Bullet>>(OnBulletDestroy);
    }

    public void SpawnBullet(Transform spawnPoint)
    {
        if (_pool.TryGetObject(out Bullet bullet))
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = spawnPoint.position;
        }
    }

    private void OnBulletDestroy(ResetSignal<Bullet> signal)
    {
        Bullet bullet = signal.Resetable;
        bullet.gameObject.SetActive(false);
        bullet.CurrentLifetime = 0;
    }
}