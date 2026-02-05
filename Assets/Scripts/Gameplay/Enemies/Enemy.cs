using Core;
using Core.Configs;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IResetable, IDespawnable
    {
        [SerializeField] private Collider2D _deadZone;

        private int _health;
        private int _maxHealth;
        private SignalBus _signalBus;
        private int _score;

        public int Score => _score;

        [Inject]
        public void Construct(EnemyConfig config, SignalBus signalBus)
        {
            _health = config.Health;
            _signalBus = signalBus;
            _score = config.Score;
        
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
            {
                if (_deadZone.IsTouching(playerMovement.GroundChecker))
                {
                    playerMovement.Jump();
                    Die();
                }
                else
                {
                    if (!playerMovement.Player.IsInvincible)
                    {
                        playerMovement.Player.Die();
                        playerMovement.ChangePlayerVelocity(0, 0);
                    }
                    else
                    {
                        Die();
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
}