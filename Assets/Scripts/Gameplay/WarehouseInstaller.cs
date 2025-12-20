using Gameplay;
using UnityEngine;
using Zenject;

public class WarehouseInstaller : MonoInstaller
{
    [SerializeField] private GameObject _platformPrefab;

    [SerializeField] private int bulletPoolCapacity;
    [SerializeField] private int enemyPoolCapacity;

    public override void InstallBindings()
    {
        GameObject platformContainer = new("PLATFORMS");

        SignalBusInstaller.Install(Container);

        InstallBullets();
        InstallEnemies();
    }

    private void InstallEnemies()
    {
        Container.DeclareSignal<ResetSignal<Enemy>>();
        GameObject enemyContainer = new("ENEMIES");
        Container.Bind<BaseFabric<Enemy>>().To<EnemyFabric>().AsSingle();
        Container.Bind<ObjectPool<Enemy>>().AsSingle().WithArguments(enemyContainer.transform, enemyPoolCapacity)
            .OnInstantiated<ObjectPool<Enemy>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<BaseSpawner<Enemy>>().AsSingle();
    }

    private void InstallBullets()
    {
        Container.DeclareSignal<ResetSignal<Bullet>>();
        GameObject bulletContainer = new("BULLETS");
        Container.Bind<BaseFabric<Bullet>>().To<BulletFabric>().AsSingle();
        Container.Bind<ObjectPool<Bullet>>().AsSingle().WithArguments(bulletContainer.transform, bulletPoolCapacity)
            .OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<BaseSpawner<Bullet>>().AsSingle();
    }
}