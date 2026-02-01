using System.Collections.Generic;
using Core;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Platforms
{
    public class BasePlatform : Platform
    {
        private SignalBus _signalBus;
        private bool _isOccupied;
        private Vector3 _springPosition;
        private Vector3 _shieldPosition;
        private List<IDespawnable> _items = new();

        public Vector3 SpringPosition => _springPosition;
        public Vector3 ShieldPosition => _shieldPosition;

        public bool IsOccupied
        {
            get => _isOccupied;
            set => _isOccupied = value;
        }

        [Inject]
        public void Construct(SignalBus signalBus, PlatformConfig config)
        {
            _signalBus = signalBus;
            _springPosition = config.SpringPosition;
            _shieldPosition = config.BoostPosition;
        }
    
        public void Add(IDespawnable item)
        {
            _items.Add(item);
        }

        public override void Reset()
        {
            foreach (var item in _items)
            {
                item.Despawn();
            }

            _items.Clear();
        }

        public override void Despawn()
        {
            _signalBus.Fire(new ResetSignal<BasePlatform>(this));
        }
    }
}