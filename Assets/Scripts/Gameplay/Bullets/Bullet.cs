using Core;
using Core.Configs;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Bullets
{
    public class Bullet : MonoBehaviour, IResetable
    {
        private int _damage;
        private int _speed;
        private float _maxLifetime;
        private SignalBus _signalBus;
    
        public float CurrentLifetime { get; private set; }

        private void Update()
        {
            Move();
            CheckLifetime();
        }

        [Inject]
        public void Construct(BulletConfig config, SignalBus signalBus)
        {
            _speed = config.Speed;
            _damage = config.Damage;
            _maxLifetime = config.MaxLifetime;
            _signalBus = signalBus;
        }
    
        private void CheckLifetime()
        {
            CurrentLifetime += Time.deltaTime;
            if (CurrentLifetime >= _maxLifetime)
            {
                DestroyBullet();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Enemies.Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                DestroyBullet();
            }
        }

        private void Move()
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        private void DestroyBullet()
        {
            _signalBus.Fire(new ResetSignal<Bullet>(this));
        }

        public void Reset()
        {
            CurrentLifetime = 0;
        }
    }
}