using Core;
using Core.Configs;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosts
{
    public class Jetpack : Boost, IResetable
    {
        private float _speed;
        private float _flightTime;
        private SignalBus _signalBus;

        public float Speed => _speed;
        public float FlightTime => _flightTime;

        [Inject]
        public void Construct(JetpackConfig config, SignalBus signalBus)
        {
            _speed = config.Speed;
            _flightTime = config.FlightTime;
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _signalBus.Fire(new PlayerGetJetpackSignal(this));
                Despawn();
            }
        }

        public override void Despawn()
        {
            _signalBus.Fire(new ResetSignal<Jetpack>(this));
        }

        public void Reset()
        {
        }
    }
}