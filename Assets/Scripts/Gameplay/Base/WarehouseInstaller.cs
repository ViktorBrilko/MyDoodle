using Cinemachine;
using Gameplay;
using Gameplay.Boosts;
using Gameplay.Chunks;
using Gameplay.Platforms;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

public class WarehouseInstaller : MonoInstaller
{
    [SerializeField] private int _bulletPoolCapacity;
    [SerializeField] private int _enemyPoolCapacity;
    [SerializeField] private int _platformPoolCapacity;
    [SerializeField] private int _brokenPlatformPoolCapacity;
    [SerializeField] private int _chunkPoolCapacity;
    [SerializeField] private int _springPoolCapacity;
    [SerializeField] private int _shieldPoolCapacity;
    [SerializeField] private int _jetpackPoolCapacity;
    [SerializeField] private Transform _chunkStartPoint;
    [SerializeField] private Transform _playerStartPoint;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerCameraPrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private GameObject _brokenPlatformPrefab;
    [SerializeField] private GameObject _springPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private GameObject _jetpackPrefab;

    private ConfigProvider _provider;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ResetSignal<Chunk>>();
        Container.DeclareSignal<ResetSignal<Enemy>>();
        Container.DeclareSignal<ResetSignal<BasePlatform>>();
        Container.DeclareSignal<ResetSignal<Bullet>>();
        Container.DeclareSignal<ResetSignal<Spring>>();
        Container.DeclareSignal<ResetSignal<Shield>>();
        Container.DeclareSignal<ResetSignal<BrokenPlatform>>();
        Container.DeclareSignal<ResetSignal<Jetpack>>();
        Container.DeclareSignal<EnemyDeadSignal>();
        Container.DeclareSignal<PlayerDiedSignal>();
        Container.DeclareSignal<PlayerGetJetpackSignal>();

        _provider = new ConfigProvider();
        _provider.LoadAll();

        InstallCamera();
        InstallBullets();
        InstallPlayer();
        InstallEnemies();
        InstallPlatforms();
        InstallSprings();
        InstallChunks();
        InstallBoosts();
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
    
    private void InstallBoosts()
    {
        GameObject shieldContainer = new("SHIELDS");
        Container.Bind<ShieldConfig>().FromInstance(_provider.ShieldCfg).AsSingle();
        Container.Bind<IFabric<Shield>>().To<Fabric<Shield, ShieldConfig>>().AsSingle().WithArguments(_shieldPrefab);
        Container.Bind<ObjectPool<Shield>>().AsSingle().WithArguments(shieldContainer.transform, _shieldPoolCapacity)
            .OnInstantiated<ObjectPool<Shield>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Shield>>().AsSingle();
        
        GameObject jetpackContainer = new("JETPACKS");
        Container.Bind<JetpackConfig>().FromInstance(_provider.JetpackCfg).AsSingle();
        Container.Bind<IFabric<Jetpack>>().To<Fabric<Jetpack, JetpackConfig>>().AsSingle().WithArguments(_jetpackPrefab);
        Container.Bind<ObjectPool<Jetpack>>().AsSingle().WithArguments(jetpackContainer.transform, _jetpackPoolCapacity)
            .OnInstantiated<ObjectPool<Jetpack>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<Jetpack>>().AsSingle();
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
        Container.Bind<IFabric<BasePlatform>>().To<Fabric<BasePlatform, PlatformConfig>>().AsSingle().WithArguments(_platformPrefab);        
        Container.Bind<ObjectPool<BasePlatform>>().AsSingle()
            .WithArguments(platformContainer.transform, _platformPoolCapacity)
            .OnInstantiated<ObjectPool<BasePlatform>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<BasePlatform>>().AsSingle();
        
        GameObject brokenPlatformContainer = new("BROKEN_PLATFORMS");
        Container.Bind<BrokenPlatformConfig>().FromInstance(_provider.BrokenPlatformCfg).AsSingle();
        Container.Bind<IFabric<BrokenPlatform>>().To<Fabric<BrokenPlatform, BrokenPlatformConfig>>().AsSingle().WithArguments(_brokenPlatformPrefab);        
        Container.Bind<ObjectPool<BrokenPlatform>>().AsSingle()
            .WithArguments(brokenPlatformContainer.transform, _brokenPlatformPoolCapacity)
            .OnInstantiated<ObjectPool<BrokenPlatform>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<Spawner<BrokenPlatform>>().AsSingle();
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