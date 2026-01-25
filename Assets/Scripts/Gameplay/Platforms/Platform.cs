using Gameplay;
using UnityEngine;
using Zenject;

public class Platform : MonoBehaviour, IResetable, IDespawnable
{
    private SignalBus _signalBus;
    private Vector3 _springPosition;

    public Vector3 SpringPosition => _springPosition;

    [Inject]
    public void Construct(SignalBus signalBus, PlatformConfig config)
    {
        _signalBus = signalBus;
        _springPosition = config.SpringPosition;
    }   

    public void Reset()
    {
    }
    
    public void Despawn()
    {
        _signalBus.Fire(new ResetSignal<Platform>(this));
    }
}