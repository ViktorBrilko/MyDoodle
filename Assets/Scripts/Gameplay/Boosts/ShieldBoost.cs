using Gameplay;
using UnityEngine;
using Zenject;

public class ShieldBoost : MonoBehaviour, IResetable, IDespawnable
{
    private int _shieldInvincibilityTime;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(ShieldConfig config, SignalBus signalBus)
    {
        _shieldInvincibilityTime = config.ShieldInvincibilityTime;
        _signalBus = signalBus;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.BecomeInvincible(_shieldInvincibilityTime);
			_signalBus.Fire(new ResetSignal<ShieldBoost>(this));
        }
    }

    public void Reset()
    {
    }

    public void Despawn()
    {
        _signalBus.Fire(new ResetSignal<ShieldBoost>(this));
    }
}
