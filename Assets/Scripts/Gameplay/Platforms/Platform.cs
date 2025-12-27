using Gameplay;
using UnityEngine;
using Zenject;

public class Platform : MonoBehaviour, IResetable, IDespawnable
{
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Reset()
    {
    }
    
    public void Despawn()
    {
        _signalBus.Fire(new ResetSignal<Platform>(this));
    }
}