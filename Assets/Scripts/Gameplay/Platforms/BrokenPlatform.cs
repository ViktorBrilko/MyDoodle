using UnityEngine;
using Zenject;

namespace Gameplay.Platforms
{
    public class BrokenPlatform : Platform, IResetable, IDespawnable
    {
        private SignalBus _signalBus;
        private BrokenPlatformConfig _config;

        [Inject]
        public void Construct(SignalBus signalBus, BrokenPlatformConfig config)
        {
            _signalBus = signalBus;
            _config = config;
          
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player) && player.Rigidbody2D.velocity.y <= 0)
            {
                Despawn();
            }
        }

        public void Reset()
        {
        }

        public override void Despawn()
        {
            _signalBus.Fire(new ResetSignal<BrokenPlatform>(this));
        }
    }
}