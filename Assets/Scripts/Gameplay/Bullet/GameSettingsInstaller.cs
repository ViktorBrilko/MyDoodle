using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private EnemyConfig _enemyConfig;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_bulletConfig).AsSingle();
        Container.BindInstance(_enemyConfig).AsSingle();
    }
}