using System.Collections.Generic;
using Core;
using Core.Configs;
using UnityEngine;

namespace Gameplay.Chunks
{
    public class ChunkLogic 
    {
        private float _yCameraOffset;
        private List<IDespawnable> _items = new();
        private List<Vector2> _itemsPositions = new();
        private int _springsInChunk;
        private int _boostsInChunk;
        private int _maxSpringsInChunk;

        public float YCameraOffset => _yCameraOffset;

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
       
        public ChunkLogic(ChunkConfig config)
        {
            _yCameraOffset = config.YCameraOffset;

            _maxSpringsInChunk = Random.Range(config.MinSprings, config.MaxSprings + 1);
        }

        public void Reset()
        {
            foreach (var item in _items)
            {
                item.Despawn();
            }

            _items.Clear();
            _itemsPositions.Clear();
            _boostsInChunk = 0;
            _springsInChunk = 0;
        }

        public void Add(IDespawnable item, Vector2 position)
        {
            _items.Add(item);
            _itemsPositions.Add(position);
        }
       
    }
}