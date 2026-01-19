using Gameplay;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IResetable, IDespawnable
{
    [SerializeField] private Collider2D _deadZone;

    private int _health;
    private int _maxHealth;
    private SignalBus _signalBus;
    private int _score;

    public int Score => _score;

    [Inject]
    public void Construct(int health, int score, SignalBus signalBus)
    {
        _health = health;
        _signalBus = signalBus;

        _maxHealth = _health;
        _score = score;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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
                if (!player.IsInvincible)
                {
                    player.Die();
                }
            }
        }
    }

    private void Die()
    {
        _signalBus.Fire(new ResetSignal<Enemy>(this));
        _signalBus.Fire(new EnemyDeadSignal(this));
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