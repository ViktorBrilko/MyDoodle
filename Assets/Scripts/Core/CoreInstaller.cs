using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _startPoint;

    public override void InstallBindings()
    {
        Container.InstantiatePrefab(_playerPrefab, _startPoint.transform.position, Quaternion.identity, null);
    }
}