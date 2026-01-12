using Gameplay;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IResetable, IDespawnable
{
    [SerializeField] private Collider2D _deadZone;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (_deadZone.IsTouching(player.GroundChecker))
            {
                player.Jump();
                Die();
            }
            else
            {
                player.Die();
            }
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