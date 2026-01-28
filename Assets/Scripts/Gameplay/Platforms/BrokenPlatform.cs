using UnityEngine;
using Zenject;

namespace Gameplay.Platforms
{
    public class BrokenPlatform : Platform
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus, BrokenPlatformConfig config)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player) && player.Rigidbody2D.velocity.y <= 0)
            {
                Despawn();
            }
        }

        public override void Despawn()
        {
            _signalBus.Fire(new ResetSignal<BrokenPlatform>(this));
        }
    }
}