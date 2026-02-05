using Core;
using Core.Configs;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosts
{
    public class Shield : Boost, IResetable
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
                Despawn();
            }
        }

        public override void Despawn()
        {
            _signalBus.Fire(new ResetSignal<Shield>(this));
        }

        public void Reset()
        {
        }
    }
}
