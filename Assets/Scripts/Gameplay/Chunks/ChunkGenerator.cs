using System;
using Core;
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
        private float _rightSideOfScreenInWorld;
        private float _leftSideOfScreenInWorld;
        private float _currentY;
        private int _numberOfChunk;
        private Vector3 _chunkSpawnPosition;
        private ObjectPool<Chunk> _pool;
        private SignalBus _signalBus;
        private ChunkConfig _config;

        private Spawner<BasePlatform> _platformSpawner;
        private Spawner<BrokenPlatform> _brokenPlatformSpawner;
        private float _platformWidthHalf;
        private float _platformXDistanceCoef;

        private Spawner<Enemy> _enemySpawner;
        private float _enemyWidthHalf;

        private Spawner<Spring> _springSpawner;
        private float _lastSpringYPosition;

        private Spawner<Shield> _shieldSpawner;
        private Spawner<Jetpack> _jetpackSpawner;

        public ChunkGenerator(ObjectPool<Chunk> pool, Transform chunkStartPoint,
            Spawner<Enemy> enemySpawner, Spawner<BasePlatform> platformSpawner,
            Spawner<BrokenPlatform> brokenPlatformSpawner, ChunkConfig chunkConfig,
            SignalBus signalBus, Spawner<Spring> springSpawner, Spawner<Shield> shieldSpawner,
            Spawner<Jetpack> jetpackSpawner,
            GameObject enemyPrefab, GameObject platformPrefab)
        {
            _rightSideOfScreenInWorld =
                Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            _leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
            _signalBus = signalBus;
            _chunkSpawnPosition = chunkStartPoint.position;
            _pool = pool;
            _config = chunkConfig;
            _platformWidthHalf = platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            _platformXDistanceCoef = _platformWidthHalf * 2 + chunkConfig.PlatformXDistanceCoef;
            _enemyWidthHalf = enemyPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            _enemySpawner = enemySpawner;
            _platformSpawner = platformSpawner;
            _springSpawner = springSpawner;
            _shieldSpawner = shieldSpawner;
            _jetpackSpawner = jetpackSpawner;
            _brokenPlatformSpawner = brokenPlatformSpawner;
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


            _chunkSpawnPosition.y += _config.ChunkHeight;
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
            _currentY = _config.ItemStartYGeneration;
            int maxEnemies = Random.Range(_config.MinEnemiesInChunk, _config.MaxEnemiesInChunk + 1);
            int enemiesInChunk = 0;

            while (_currentY <= _config.ChunkHeight && enemiesInChunk < maxEnemies)
            {
                bool isValidPosition = false;
                int attempts = 0;
                Vector3 candidatePosition = new Vector3();

                while (!isValidPosition && attempts <= _config.MaxPositionAttempts)
                {
                    candidatePosition = new Vector3(
                        Random.Range(_rightSideOfScreenInWorld - _enemyWidthHalf,
                            _leftSideOfScreenInWorld + _enemyWidthHalf),
                        _currentY, chunk.transform.position.z);

                    if (chunk.ItemsPositions.Count == 0) break;

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

                    if (attempts >= _config.MaxPositionAttempts)
                    {
                        Debug.Log("Количество попыток разместить врага исчерпано");
                    }
                }

                Enemy enemy = _enemySpawner.SpawnItem(candidatePosition);
                enemy.transform.SetParent(chunk.transform);

                enemy.transform.localPosition = candidatePosition;
                chunk.Add(enemy, enemy.transform.localPosition);
                enemiesInChunk++;

                int changeY = Random.Range(_config.MinYDistanceBetweenEnemies, _config.MaxYDistanceBetweenEnemies);
                if (_currentY + changeY > _config.ChunkHeight)
                {
                    changeY = _config.MinYDistanceBetweenEnemies;
                }

                _currentY += changeY;
            }
        }

        private void SpawnPlatforms(Chunk chunk)
        {
            _currentY = _config.ItemStartYGeneration;
            _lastSpringYPosition = 0;
            int lastYChange = 0;
            bool isLastPlatformBroken = false;

            while (_currentY <= _config.ChunkHeight)
            {
                bool isValidPosition = false;
                int attempts = 0;
                Vector3 candidatePosition = new Vector3();

                while (!isValidPosition && attempts <= _config.MaxPositionAttempts)
                {
                    candidatePosition = new Vector3(
                        Random.Range(_rightSideOfScreenInWorld - _platformWidthHalf,
                            _leftSideOfScreenInWorld + _platformWidthHalf),
                        _currentY, chunk.transform.position.z);

                    if (chunk.ItemsPositions.Count == 0) break;

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

                    if (attempts >= _config.MaxPositionAttempts)
                    {
                        Debug.Log("Количество попыток разместить платформу исчерпано");
                    }
                }

                Platform platform;

                int chance = Random.Range(0, 100);
                if (chance < _config.BrokenPlatformChance && lastYChange != _config.BigChangeY &&
                    !isLastPlatformBroken && _currentY != _config.ItemStartYGeneration)
                {
                    platform = _brokenPlatformSpawner.SpawnItem(candidatePosition);
                    isLastPlatformBroken = true;
                }
                else
                {
                    platform = _platformSpawner.SpawnItem(candidatePosition);
                    isLastPlatformBroken = false;
                }

                platform.transform.SetParent(chunk.transform);

                platform.transform.localPosition = candidatePosition;
                chunk.Add(platform, platform.transform.localPosition);

                BasePlatform basePlatform = platform as BasePlatform;
                if (basePlatform != null)
                {
                    TrySpawnSpring(basePlatform, chunk);
                    TrySpawnBoost(basePlatform, chunk);
                }

                lastYChange = ChangeYRow(isLastPlatformBroken);
                _currentY += lastYChange;
            }
        }

        private void TrySpawnSpring(BasePlatform platform, Chunk chunk)
        {
            if (chunk.SpringsInChunk >= chunk.MaxSpringsInChunk || platform.IsOccupied) return;

            if (_currentY - _lastSpringYPosition < _config.MinYDistanceBetweenSprings) return;

            int chance = Random.Range(0, 100);
            if (chance > _config.SpringedPlatformChance) return;

            Spring spring = _springSpawner.SpawnItem(new Vector3());
            spring.transform.SetParent(platform.transform);
            platform.Add(spring);
            spring.transform.localPosition = platform.SpringPosition;
            platform.IsOccupied = true;
            _lastSpringYPosition = _currentY;
            chunk.SpringsInChunk++;
        }

        private void TrySpawnBoost(BasePlatform basePlatform, Chunk chunk)
        {
            if (chunk.BoostsInChunk >= _config.MaxBoosts || basePlatform.IsOccupied) return;

            int chance = Random.Range(0, 100);
            if (chance > _config.BoostedPlatformChance) return;

            Boost boost;

            chance = Random.Range(0, 100);
            if (0 < chance && chance <= _config.ShieldChance)
            {
                boost = _shieldSpawner.SpawnItem(new Vector3());
            }
            else 
            {
                boost = _jetpackSpawner.SpawnItem(new Vector3());
            }

            boost.transform.SetParent(basePlatform.transform);
            chunk.Add(boost, boost.transform.localPosition);
            boost.transform.localPosition = basePlatform.ShieldPosition;

            basePlatform.IsOccupied = true;
            chunk.BoostsInChunk++;
        }

        private int ChangeYRow(bool isLastPlatformBroken)
        {
            int chance = Random.Range(0, 100);
            int res = _config.DefaultChangeY;

            if (chance >= 0 && chance <= _config.LeaveYChance)
            {
                //ряд не меняется
                res = 0;
            }
            else if (_config.LeaveYChance < chance && chance <= _config.BigChangeYChance && !isLastPlatformBroken)
            {
                //ряд меняется на несколько и часть из них остаются пустыми
                return _config.BigChangeY;
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

            for (int i = 0; i < _config.InitialChunksCount; i++)
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