using UnityEngine;
using Zenject;

public class WarehouseInstaller : MonoInstaller
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _platformPrefab;
    
    [SerializeField] private int bulletPoolCapacity;

    public override void InstallBindings()
    {
        GameObject bulletContainer = new("BULLETS");
        GameObject enemyContainer = new("ENEMIES");
        GameObject platformContainer = new("PLATFORMS");
        
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ResetSignal<Bullet>>();
        
        Container.Bind<IFabric<Bullet>>().To<BulletFabric>().AsSingle().WithArguments(_bulletPrefab);
        Container.Bind<ObjectPool<Bullet>>().AsSingle().
            WithArguments(bulletContainer.transform, bulletPoolCapacity).OnInstantiated<ObjectPool<Bullet>>((c, p) => p.Initialize());
        Container.BindInterfacesAndSelfTo<BulletSpawner>().AsSingle();
       

	


    }
}