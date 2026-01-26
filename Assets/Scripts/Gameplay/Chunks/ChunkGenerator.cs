using System;
using Gameplay.Chunks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ChunkGenerator : IInitializable, IDisposable
    {
        private float _rightSideOfScreenInWorld;
        private float _leftSideOfScreenInWorld;
        private float _chunkHeight;
        private int _maxPositionAttempts;
        private int _initialChunksCount;
        private Vector3 _chunkSpawnPosition;
        private float _currentY;
        private ObjectPool<Chunk> _pool;
        private SignalBus _signalBus;
        private int _numberOfChunk;
        private int _itemStartYGeneration;
        private ChunkConfig _config;

        private Spawner<Platform> _platformSpawner;
        private int _leaveYChance;
        private int _bigChangeYChance;
        private int _bigChangeY;
        private int _defaultChangeY;
        private float _platformWidthHalf;
        private float _platformXDistanceCoef;

        private Spawner<Enemy> _enemySpawner;
        private int _maxEnemiesInChunk;
        private int _minEnemiesInChunk;
        private float _enemyWidthHalf;
        private int _maxYDistanceBetweenEnemies;
        private int _minYDistanceBetweenEnemies;

        private Spawner<Spring> _springSpawner;
        private int _springedPlatformChance;
        private float _lastSpringYPosition;

        private Spawner<ShieldBoost> _shieldSpawner;

        public ChunkGenerator(ObjectPool<Chunk> pool, Transform chunkStartPoint,
            Spawner<Enemy> enemySpawner, Spawner<Platform> platformSpawner, ChunkConfig chunkConfig,
            SignalBus signalBus, Spawner<Spring> springSpawner, Spawner<ShieldBoost> shieldSpawner,
            GameObject enemyPrefab, GameObject platformPrefab)
        {
            _rightSideOfScreenInWorld =
                Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            _leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
            _signalBus = signalBus;
            _chunkSpawnPosition = chunkStartPoint.position;
            _pool = pool;
            _initialChunksCount = chunkConfig.InitialChunksCount;
            _maxPositionAttempts = chunkConfig.MaxPositionAttempts;
            _chunkHeight = chunkConfig.ChunkHeight;
            _itemStartYGeneration = chunkConfig.ItemStartYGeneration;
            _config = chunkConfig;

            _platformSpawner = platformSpawner;
            _leaveYChance = chunkConfig.LeaveYChance;
            _bigChangeYChance = chunkConfig.BigChangeYChance;
            _bigChangeY = chunkConfig.BigChangeY;
            _defaultChangeY = chunkConfig.DefaultChangeY;
            _platformWidthHalf = platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            _platformXDistanceCoef = _platformWidthHalf + chunkConfig.PlatformXDistanceCoef;

            _enemySpawner = enemySpawner;
            _maxEnemiesInChunk = chunkConfig.MaxEnemiesInChunk;
            _minEnemiesInChunk = chunkConfig.MinEnemiesInChunk;
            _enemyWidthHalf = enemyPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            _maxYDistanceBetweenEnemies = chunkConfig.MaxYDistanceBetweenEnemies;
            _minYDistanceBetweenEnemies = chunkConfig.MinYDistanceBetweenEnemies;

            _springSpawner = springSpawner;
            _springedPlatformChance = chunkConfig.SpringedPlatformChance;

            _shieldSpawner = shieldSpawner;
        }

        private void SpawnChunk(Vector3 position)
        {
            Chunk newChunk;

            if (_pool.TryGetObject(out Chunk chunk))
            {
                newChunk = chunk;
            }
            else
            {
                newChunk = _pool.AddNewItem();
            }

            newChunk.gameObject.SetActive(true);
            newChunk.transform.position = position;
            FillChunk(newChunk);


            _chunkSpawnPosition.y += _chunkHeight;
            newChunk.endOfChunk.transform.position = new Vector3(0, _chunkSpawnPosition.y, 1);
            _numberOfChunk++;
        }

        private void FillChunk(Chunk chunk)
        {
            SpawnPlatforms(chunk);

            //чтобы сразу в первом чанке не спавнить врагов
            if (_numberOfChunk != 0)
            {
                SpawnEnemies(chunk);
            }
        }

        private void SpawnEnemies(Chunk chunk)
        {
            _currentY = _itemStartYGeneration;
            int maxEnemies = Random.Range(_minEnemiesInChunk, _maxEnemiesInChunk + 1);
            int enemiesInChunk = 0;

            while (_currentY <= _chunkHeight && enemiesInChunk < maxEnemies)
            {
                bool isValidPosition = false;
                int attempts = 0;
                Vector3 candidatePosition = new Vector3();

                while (!isValidPosition && attempts <= _maxPositionAttempts)
                {
                    candidatePosition = new Vector3(
                        Random.Range(_rightSideOfScreenInWorld - _enemyWidthHalf,
                            _leftSideOfScreenInWorld + _enemyWidthHalf),
                        _currentY, chunk.transform.position.z);

                    if (chunk.ItemsPositions.Count == 0)
                    {
                        break;
                    }

                    foreach (Vector2 existingPosition in chunk.ItemsPositions)
                    {
                        if (candidatePosition.y == existingPosition.y &&
                            Mathf.Abs(candidatePosition.x - existingPosition.x) < _platformXDistanceCoef)
                        {
                            attempts++;
                            isValidPosition = false;
                            break;
                        }

                        isValidPosition = true;
                    }

                    if (attempts >= _maxPositionAttempts)
                    {
                        Debug.Log("Количество попыток разместить врага исчерпано");
                    }
                }

                Enemy enemy = _enemySpawner.SpawnItem(candidatePosition);
                enemy.transform.SetParent(chunk.transform);

                enemy.transform.localPosition = candidatePosition;
                chunk.Add(enemy, enemy.transform.localPosition);
                enemiesInChunk++;

                int changeY = Random.Range(_minYDistanceBetweenEnemies, _maxYDistanceBetweenEnemies);
                if (_currentY + changeY > _chunkHeight)
                {
                    changeY = _minYDistanceBetweenEnemies;
                }

                _currentY += changeY;
            }
        }

        private void SpawnPlatforms(Chunk chunk)
        {
            _currentY = _itemStartYGeneration;
            _lastSpringYPosition = 0;

            while (_currentY <= _chunkHeight)
            {
                bool isValidPosition = false;
                int attempts = 0;
                Vector3 candidatePosition = new Vector3();

                while (!isValidPosition && attempts <= _maxPositionAttempts)
                {
                    candidatePosition = new Vector3(
                        Random.Range(_rightSideOfScreenInWorld - _platformWidthHalf,
                            _leftSideOfScreenInWorld + _platformWidthHalf),
                        _currentY, chunk.transform.position.z);

                    if (chunk.ItemsPositions.Count == 0)
                    {
                        break;
                    }

                    foreach (Vector2 existingPosition in chunk.ItemsPositions)
                    {
                        if (candidatePosition.y == existingPosition.y &&
                            Mathf.Abs(candidatePosition.x - existingPosition.x) < _platformXDistanceCoef)
                        {
                            attempts++;
                            isValidPosition = false;
                            break;
                        }

                        isValidPosition = true;
                    }

                    if (attempts >= _maxPositionAttempts)
                    {
                        Debug.Log("Количество попыток разместить платформу исчерпано");
                    }
                }

                Platform platform = _platformSpawner.SpawnItem(candidatePosition);

                TrySpawnSpring(platform, chunk);
                TrySpawnBoost(platform, chunk);

                platform.transform.SetParent(chunk.transform);

                platform.transform.localPosition = candidatePosition;
                chunk.Add(platform, platform.transform.localPosition);
                _currentY += ChangeYRow();
            }
        }

        private void TrySpawnSpring(Platform platform, Chunk chunk)
        {
            if (chunk.SpringsInChunk >= chunk.MaxSpringsInChunk || platform.IsOccupied) return;
            
            if(_currentY - _lastSpringYPosition < _config.MinYDistanceBetweenSprings) return;

            int chance = Random.Range(0, 100);
            if (chance > _springedPlatformChance) return;

            Spring spring = _springSpawner.SpawnItem(new Vector3());
            spring.transform.SetParent(platform.transform);
            platform.Add(spring);
            spring.transform.localPosition = platform.SpringPosition;
            platform.IsOccupied = true;
            _lastSpringYPosition = _currentY;
            chunk.SpringsInChunk++;
        }

        private void TrySpawnBoost(Platform platform, Chunk chunk)
        {
            if (chunk.BoostsInChunk >= _config.MaxBoosts || platform.IsOccupied) return;

            int chance = Random.Range(0, 100);
            if (chance > _config.BoostedPlatformChance) return;

            ShieldBoost shield = _shieldSpawner.SpawnItem(new Vector3());
            shield.transform.SetParent(platform.transform);
            platform.Add(shield);
            platform.IsOccupied = true;
            shield.transform.localPosition = platform.ShieldPosition;
            chunk.BoostsInChunk++;
        }

        private int ChangeYRow()
        {
            int chance = Random.Range(0, 100);
            int res = _defaultChangeY;

            if (chance >= 0 && chance <= _leaveYChance)
            {
                //ряд не меняется
                res = 0;
            }
            else if (_leaveYChance < chance && chance <= _bigChangeYChance)
            {
                //ряд меняется на несколько и часть из них остаются пустыми
                return _bigChangeY;
            }

            return res;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ResetSignal<Chunk>>(OnItemDestroy);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ResetSignal<Chunk>>(OnItemDestroy);

            for (int i = 0; i < _initialChunksCount; i++)
            {
                SpawnChunk(_chunkSpawnPosition);
            }
        }

        private void OnItemDestroy(ResetSignal<Chunk> signal)
        {
            Chunk item = signal.Resetable;
            item.gameObject.SetActive(false);
            item.Reset();
            SpawnChunk(_chunkSpawnPosition);
        }
    }
}