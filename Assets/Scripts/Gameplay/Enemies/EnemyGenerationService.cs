using Core.Configs;
using Gameplay.Base;
using Gameplay.Chunks;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyGenerationService
    {
        private Spawner<Enemy> _enemySpawner;
        private ChunkConfig _config;
        private PositionValidationService _positionValidationService;
        private float _platformWidthHalf;

        public EnemyGenerationService(ChunkConfig config, Spawner<Enemy> enemySpawner,
            PositionValidationService positionValidationService, GameObject platformPrefab)
        {
            _config = config;
            _enemySpawner = enemySpawner;
            _positionValidationService = positionValidationService;
            _platformWidthHalf = platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }

        public void SpawnEnemies(ChunkPresentation chunkPresentation)
        {
            float currentY = _config.ItemStartYGeneration;
            int maxEnemies = Random.Range(_config.MinEnemiesInChunk, _config.MaxEnemiesInChunk + 1);
            int enemiesInChunk = 0;

            while (currentY <= _config.ChunkHeight && enemiesInChunk < maxEnemies)
            {
                Vector3 candidatePosition =
                    _positionValidationService.GetValidPosition(_platformWidthHalf, currentY, chunkPresentation);

                Enemy enemy = CreateEnemy(candidatePosition);
                enemy.transform.SetParent(chunkPresentation.transform);

                enemy.transform.localPosition = candidatePosition;
                chunkPresentation.Logic.Add(enemy, enemy.transform.localPosition);
                enemiesInChunk++;

                int changeY = Random.Range(_config.MinYDistanceBetweenEnemies, _config.MaxYDistanceBetweenEnemies);
                if (currentY + changeY > _config.ChunkHeight)
                {
                    changeY = _config.MinYDistanceBetweenEnemies;
                }

                currentY += changeY;
            }
        }
        
        private Enemy CreateEnemy(Vector3 position)
        {
            return _enemySpawner.SpawnItem(position);
        }
    }
}