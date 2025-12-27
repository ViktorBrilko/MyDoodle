using Gameplay;
using Gameplay.Chunks;
using UnityEngine;
using Zenject;

public class WarehouseInstaller : MonoInstaller
{
    [SerializeField] private int _bulletPoolCapacity;
    [SerializeField] private int _enemyPoolCapacity;
    [SerializeField] private int _platformPoolCapacity;
    [SerializeField] private int _chunkPoolCapacity;
    [SerializeField] private Transform _chunkStartPoint;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerStartPoint;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ResetSignal<Chunk>>();
        Container.DeclareSignal<ResetSignal<Enemy>>();
        Container.DeclareSignal<ResetSignal<Platform>>();
        Container.DeclareSignal<ResetSignal<Bullet>>();

        InstallBullets();
        Container.InstantiatePrefab(_playerPrefab, _playerStartPoint.transform.position, Quaternion.identity, null);
        InstallEnemies();
        InstallPlatforms();
        InstallChunks();
    }

    private void InstallChunks()
    {
        
        GameObject chunkContainer = new("CHUNKS");
        Container.Bind<BaseFabric<Chunk>>().To<ChunkFabric>().AsSingle();
        Container.Bind<ObjectPool<Chunk>>().AsSingle().WithArguments(chunkContainer.transform, _chunkPoolCapacity)
            .OnInstantiated<ObjectPool<Chunk>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<ChunkGenerator>().AsSingle().WithArguments(_chunkStartPoint);
    }

    private void InstallEnemies()
    {
        GameObject enemyContainer = new("ENEMIES");
        Container.Bind<BaseFabric<Enemy>>().To<EnemyFabric>().AsSingle();
        Container.Bind<ObjectPool<Enemy>>().AsSingle().WithArguments(enemyContainer.transform, _enemyPoolCapacity)
            .OnInstantiated<ObjectPool<Enemy>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Enemy>>().AsSingle();
    }

    private void InstallPlatforms()
    {
       
        GameObject platformContainer = new("PLATFORMS");
        Container.Bind<BaseFabric<Platform>>().To<PlatformFabric>().AsSingle();
        Container.Bind<ObjectPool<Platform>>().AsSingle()
            .WithArguments(platformContainer.transform, _platformPoolCapacity)
            .OnInstantiated<ObjectPool<Platform>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Platform>>().AsSingle();
    }

    private void InstallBullets()
    {
       
        GameObject bulletContainer = new("BULLETS");
        Container.Bind<BaseFabric<Bullet>>().To<BulletFabric>().AsSingle();
        Container.Bind<ObjectPool<Bullet>>().AsSingle().WithArguments(bulletContainer.transform, _bulletPoolCapacity)
            .OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Bullet>>().AsSingle();
    }
}