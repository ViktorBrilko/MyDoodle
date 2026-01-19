using Gameplay.Chunks;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private PlatformConfig _platformConfig;
    [SerializeField] private ChunkConfig _chunkConfig;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private BoostConfig _boostConfig;
    [SerializeField] private SpringConfig _springConfig;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_bulletConfig).AsSingle();
        Container.BindInstance(_enemyConfig).AsSingle();
        Container.BindInstance(_platformConfig).AsSingle();
        Container.BindInstance(_chunkConfig).AsSingle();
        Container.BindInstance(_playerConfig).AsSingle();
        Container.BindInstance(_boostConfig).AsSingle();
        Container.BindInstance(_springConfig).AsSingle();
    }
}