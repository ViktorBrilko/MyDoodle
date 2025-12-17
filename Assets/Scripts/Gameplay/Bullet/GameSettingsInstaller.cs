using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private BulletConfig _bulletConfig;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_bulletConfig).AsSingle();
    }
}