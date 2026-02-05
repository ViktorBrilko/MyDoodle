using System;
using Core;
using Core.Configs;
using Gameplay.Base;
using Gameplay.Boosts;
using Gameplay.Enemies;
using Gameplay.Platforms;
using Gameplay.Signals;
using Gameplay.Springs;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Chunks
{
    public class ChunkGenerator : IInitializable, IDisposable
    {
        private int _numberOfChunk;
        private Vector3 _chunkSpawnPosition;
        private ObjectPool<ChunkPresentation> _pool;
        private SignalBus _signalBus;
        private ChunkConfig _config;

        private PlatformGenerationService _platformGenerationService;
        private EnemyGenerationService _enemyGenerationService;

        public ChunkGenerator(ObjectPool<ChunkPresentation> pool, Transform chunkStartPoint,
            ChunkConfig chunkConfig, PlatformGenerationService platformGenerationService,
            SignalBus signalBus, EnemyGenerationService enemyGenerationService)
        {
            _signalBus = signalBus;
            _chunkSpawnPosition = chunkStartPoint.position;
            _pool = pool;
            _config = chunkConfig;

            _enemyGenerationService = enemyGenerationService;
            _platformGenerationService = platformGenerationService;
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<ResetSignal<ChunkPresentation>>(OnItemDestroy);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ResetSignal<ChunkPresentation>>(OnItemDestroy);

            for (int i = 0; i < _config.InitialChunksCount; i++)
            {
                SpawnChunk(_chunkSpawnPosition);
            }
        }

        private void SpawnChunk(Vector3 position)
        {
            ChunkPresentation newChunkPresentation;

            if (_pool.TryGetObject(out ChunkPresentation chunk))
            {
                newChunkPresentation = chunk;
            }
            else
            {
                newChunkPresentation = _pool.AddNewItem();
            }

            newChunkPresentation.gameObject.SetActive(true);
            newChunkPresentation.transform.position = position;
            FillChunk(newChunkPresentation);

            _chunkSpawnPosition.y += _config.ChunkHeight;
            _numberOfChunk++;
        }

        private void FillChunk(ChunkPresentation chunkPresentation)
        {
            _platformGenerationService.SpawnPlatforms(chunkPresentation);

            //чтобы сразу в первом чанке не спавнить врагов
            if (_numberOfChunk != 0)
            {
                _enemyGenerationService.SpawnEnemies(chunkPresentation);
            }
        }

        private void OnItemDestroy(ResetSignal<ChunkPresentation> signal)
        {
            ChunkPresentation item = signal.Resetable;
            item.gameObject.SetActive(false);
            item.Reset();
            SpawnChunk(_chunkSpawnPosition);
        }
    }
}