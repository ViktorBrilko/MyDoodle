using System.Collections.Generic;
using Core;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Chunks
{
    public class Chunk : MonoBehaviour, IResetable
    {
        private SignalBus _signalBus;
        private float _yCameraOffset;
        private List<IDespawnable> _items = new();
        private List<Vector2> _itemsPositions = new();
        private int _springsInChunk;
        private int _boostsInChunk;
        private int _maxSpringsInChunk;

        public List<Vector2> ItemsPositions => _itemsPositions;

        public int MaxSpringsInChunk => _maxSpringsInChunk;

        public int SpringsInChunk
        {
            get => _springsInChunk;
            set => _springsInChunk = value;
        }

        public int BoostsInChunk
        {
            get => _boostsInChunk;
            set => _boostsInChunk = value;
        }

        [Inject]
        public void Construct(ChunkConfig config, SignalBus signalBus)
        {
            _yCameraOffset = config.YCameraOffset;
            _signalBus = signalBus;

            _maxSpringsInChunk = Random.Range(config.MinSprings, config.MaxSprings + 1);
        }

        public void Add(IDespawnable item, Vector2 position)
        {
            _items.Add(item);
            _itemsPositions.Add(position);
        }

        private void Update()
        {
            if (CheckChunkVisibility())
            {
                TurnOff();
            }
        }

        private bool CheckChunkVisibility()
        {
            return Camera.main.WorldToViewportPoint(transform.position).y <= _yCameraOffset;
        }

        private void TurnOff()
        {
            _signalBus.Fire(new ResetSignal<Chunk>(this));
        }

        public void Reset()
        {
            foreach (var item in _items)
            {
                item.Despawn();
            }

            _items.Clear();
            _itemsPositions.Clear();
        }
    }
}