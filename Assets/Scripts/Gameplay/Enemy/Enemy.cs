using Gameplay;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IResetable, IDespawnable
{
    private int _health;
    private int _maxHealth;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(int health, SignalBus signalBus)
    {
        _health = health;
        _signalBus = signalBus;
        
        _maxHealth = _health;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _signalBus.Fire(new ResetSignal<Enemy>(this));
    }

    public void Reset()
    {
        _health = _maxHealth;
    }

    public void Despawn()
    {
        _signalBus.Fire(new ResetSignal<Enemy>(this));
    }
}