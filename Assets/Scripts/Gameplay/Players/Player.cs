using System;
using System.Collections;
using Gameplay.Base;
using Gameplay.Bullets;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _bulletSpawnPoint;

        private Spawner<Bullet> _bulletSpawner;
        private SpriteRenderer _renderer;
        private bool _isAlive = true;
        private bool _isInvincible;
        private SignalBus _signalBus;
        private Color _regularColor;

        public bool IsInvincible => _isInvincible;
        public bool IsAlive => _isAlive;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _regularColor = _renderer.color;
        }

        [Inject]
        public void Construct(SignalBus signalBus, Spawner<Bullet> spawner)
        {
            transform.SetParent(null);
            _bulletSpawner = spawner;
            _signalBus = signalBus;
        }

        public void BecomeInvincible(float invincibilityTime)
        {
            StopCoroutine(InvincibilityRoutine(invincibilityTime));
            ResetInvincibility();
            StartCoroutine(InvincibilityRoutine(invincibilityTime));
        }

        public void ResetInvincibility()
        {
            _isInvincible = false;
            _renderer.color = _regularColor;
        }

        public void Fire()
        {
            _bulletSpawner.SpawnItem(_bulletSpawnPoint.position);
        }

        public void Die()
        {
            GetComponent<PlayerInputHandler>().enabled = false;
            _isAlive = false;
            _isInvincible = false;
            _signalBus.Fire(new PlayerDiedSignal());
        }
        
        private IEnumerator InvincibilityRoutine(float invincibilityTime)
        {
            _isInvincible = true;
            _renderer.color = Color.magenta;
            yield return new WaitForSeconds(invincibilityTime);
            _renderer.color = _regularColor;
            _isInvincible = false;
        }
    }
}