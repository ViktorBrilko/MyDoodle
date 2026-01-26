using Cinemachine;
using Gameplay;
using Gameplay.Chunks;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

public class WarehouseInstaller : MonoInstaller
{
    [SerializeField] private int _bulletPoolCapacity;
    [SerializeField] private int _enemyPoolCapacity;
    [SerializeField] private int _platformPoolCapacity;
    [SerializeField] private int _chunkPoolCapacity;
    [SerializeField] private int _springPoolCapacity;
    [SerializeField] private int _shieldPoolCapacity;
    [SerializeField] private Transform _chunkStartPoint;
    [SerializeField] private Transform _playerStartPoint;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerCameraPrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private GameObject _springPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _chunkPrefab;

    private ConfigProvider _provider;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ResetSignal<Chunk>>();
        Container.DeclareSignal<ResetSignal<Enemy>>();
        Container.DeclareSignal<ResetSignal<Platform>>();
        Container.DeclareSignal<ResetSignal<Bullet>>();
        Container.DeclareSignal<ResetSignal<Spring>>();
        Container.DeclareSignal<ResetSignal<ShieldBoost>>();
        Container.DeclareSignal<EnemyDeadSignal>();
        Container.DeclareSignal<PlayerDiedSignal>();

        _provider = new ConfigProvider();
        _provider.LoadAll();

        InstallCamera();
        InstallBullets();
        InstallPlayer();
        InstallEnemies();
        InstallPlatforms();
        InstallSprings();
        InstallChunks();
        InstallShields();
    }

    private void InstallCamera()
    {
        Container.Bind<CinemachineVirtualCamera>().FromComponentInNewPrefab(_playerCameraPrefab)
            .AsSingle().NonLazy();
    }

    private void InstallPlayer()
    {
        Container.Bind<Player>().FromComponentInNewPrefab(_playerPrefab).UnderTransform(_playerStartPoint.transform)
            .AsSingle().NonLazy();
        Container.Bind<PlayerConfig>().FromInstance(_provider.PlayerCfg).AsSingle();
    }
    
    private void InstallShields()
    {
        GameObject shieldContainer = new("SHIELDS");
        Container.Bind<ShieldConfig>().FromInstance(_provider.ShieldCfg).AsSingle();
        Container.Bind<IFabric<ShieldBoost>>().To<Fabric<ShieldBoost, ShieldConfig>>().AsSingle().WithArguments(_shieldPrefab);
        Container.Bind<ObjectPool<ShieldBoost>>().AsSingle().WithArguments(shieldContainer.transform, _shieldPoolCapacity)
            .OnInstantiated<ObjectPool<ShieldBoost>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<ShieldBoost>>().AsSingle();
    }

    private void InstallChunks()
    {
        GameObject chunkContainer = new("CHUNKS");
        Container.Bind<ChunkConfig>().FromInstance(_provider.ChunkCfg).AsSingle();
        Container.Bind<IFabric<Chunk>>().To<Fabric<Chunk, ChunkConfig>>().AsSingle().WithArguments(_chunkPrefab);
        Container.Bind<ObjectPool<Chunk>>().AsSingle().WithArguments(chunkContainer.transform, _chunkPoolCapacity)
            .OnInstantiated<ObjectPool<Chunk>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<ChunkGenerator>().AsSingle().WithArguments(_chunkStartPoint, _enemyPrefab, _platformPrefab);
    }

    private void InstallEnemies()
    {
        GameObject enemyContainer = new("ENEMIES");
        Container.Bind<EnemyConfig>().FromInstance(_provider.EnemyCfg).AsSingle();
        Container.Bind<IFabric<Enemy>>().To<Fabric<Enemy, EnemyConfig>>().AsSingle().WithArguments(_enemyPrefab);
        Container.Bind<ObjectPool<Enemy>>().AsSingle().WithArguments(enemyContainer.transform, _enemyPoolCapacity)
            .OnInstantiated<ObjectPool<Enemy>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Enemy>>().AsSingle();
    }

    private void InstallPlatforms()
    {
        GameObject platformContainer = new("PLATFORMS");
        Container.Bind<PlatformConfig>().FromInstance(_provider.PlatformCfg).AsSingle();
        Container.Bind<IFabric<Platform>>().To<Fabric<Platform, PlatformConfig>>().AsSingle().WithArguments(_platformPrefab);        
        Container.Bind<ObjectPool<Platform>>().AsSingle()
            .WithArguments(platformContainer.transform, _platformPoolCapacity)
            .OnInstantiated<ObjectPool<Platform>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Platform>>().AsSingle();
    }

    private void InstallSprings()
    {
        GameObject springContainer = new("SPRINGS");
        Container.Bind<SpringConfig>().FromInstance(_provider.SpringCfg).AsSingle();
        Container.Bind<IFabric<Spring>>().To<Fabric<Spring, SpringConfig>>().AsSingle().WithArguments(_springPrefab);
        Container.Bind<ObjectPool<Spring>>().AsSingle()
            .WithArguments(springContainer.transform, _springPoolCapacity)
            .OnInstantiated<ObjectPool<Spring>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Spring>>().AsSingle();
    }

    private void InstallBullets()
    {
        GameObject bulletContainer = new("BULLETS");
        Container.Bind<BulletConfig>().FromInstance(_provider.BulletCfg).AsSingle();
        Container.Bind<IFabric<Bullet>>().To<Fabric<Bullet, BulletConfig>>().AsSingle().WithArguments(_bulletPrefab);
        Container.Bind<ObjectPool<Bullet>>().AsSingle().WithArguments(bulletContainer.transform, _bulletPoolCapacity)
            .OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Bullet>>().AsSingle();
    }
}