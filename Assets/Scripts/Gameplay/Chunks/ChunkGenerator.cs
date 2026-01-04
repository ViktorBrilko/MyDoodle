using System;
using Gameplay.Chunks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ChunkGenerator : IInitializable, IDisposable
    {
        private float _rightSideOfScreenInWorld =
            Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;

        private float _leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;

        private float _chunkHeight;
        private int _leaveYChance;
        private int _bigChangeYChance;
        private int _bigChangeY;
        private int _defaultChangeY;
        private int _maxPositionAttempts;

        private int _initialChunksCount;
        private Vector3 _spawnPosition;
        private float _currentY = 1;
        private float _platformWidthHalf;
        private float _platformXDistanceCoef;

        private Spawner<Enemy> _enemySpawner;
        private Spawner<Platform> _platformSpawner;
        private ObjectPool<Chunk> _pool;
        private SignalBus _signalBus;
        private Transform _chunkStartPoint;
        private PlayerConfig _playerConfig;

        public ChunkGenerator(ObjectPool<Chunk> pool, PlayerConfig playerConfig, Transform chunkStartPoint,
            Spawner<Enemy> enemySpawner, Spawner<Platform> platformSpawner, ChunkConfig chunkConfig,
            PlatformConfig platformConfig,
            SignalBus signalBus)
        {
            _enemySpawner = enemySpawner;
            _platformSpawner = platformSpawner;
            _signalBus = signalBus;
            _chunkStartPoint = chunkStartPoint;
            _playerConfig = playerConfig;
            _spawnPosition = _chunkStartPoint.position;
            _pool = pool;

            _chunkHeight = chunkConfig.ChunkHeight;
            _leaveYChance = chunkConfig.LeaveYChance;
            _bigChangeYChance = chunkConfig.BigChangeYChance;
            _initialChunksCount = chunkConfig.InitialChunksCount;
            _maxPositionAttempts = chunkConfig.MaxPositionAttempts;

            _bigChangeY = chunkConfig.BigChangeY;
            _defaultChangeY = chunkConfig.DefaultChangeY;
            _platformWidthHalf = platformConfig.Width / 2;
            _platformXDistanceCoef = platformConfig.Width * chunkConfig.PlatformXDistanceCoef;
        }

        private void SpawnChunk(Vector3 position)
        {
            _pool.TryGetObject(out Chunk chunk);
            chunk.gameObject.SetActive(true);
            chunk.transform.position = position;
            FillChunk(chunk);

            _spawnPosition.y += _chunkHeight;
        }

        private void FillChunk(Chunk chunk)
        {
            while (_currentY <= _chunkHeight)
            {
                bool isValidPosition = false;
                int attempts = 0;
                Vector3 candidatePosition = new Vector3(
                    Random.Range(_rightSideOfScreenInWorld - _platformWidthHalf,
                        _leftSideOfScreenInWorld + _platformWidthHalf),
                    _currentY, chunk.transform.position.z);

                if (chunk.ItemsPositions.Count > 0)
                {
                    while (!isValidPosition && attempts <= _maxPositionAttempts)
                    {
                        candidatePosition = new Vector3(
                            Random.Range(_rightSideOfScreenInWorld - _platformWidthHalf,
                                _leftSideOfScreenInWorld + _platformWidthHalf),
                            _currentY, chunk.transform.position.z);

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
                            Debug.Log("Количество попыток исчерпано");
                        }
                    }
                }

                Platform platform = _platformSpawner.SpawnItem(candidatePosition);
                platform.transform.SetParent(chunk.transform);

                platform.transform.localPosition = candidatePosition;
                chunk.Add(platform, platform.transform.localPosition);
                _currentY += ChangeYRow();
            }

            _currentY = 1;
        }

        private int ChangeYRow()
        {
            int chance = Random.Range(0, 100);
            float emptyRows = 0;
            int res = 0;

            if (chance <= _leaveYChance)
            {
                //ряд не меняется
                Debug.Log("ряд не изменился");
            }
            else if (_leaveYChance < chance && chance <= _bigChangeYChance)
            {
                Debug.Log(_playerConfig.MaxJumpHeight);
                if (emptyRows < _playerConfig.MaxJumpHeight)
                {
                    emptyRows += _defaultChangeY;
                    //ряд меняется на несколько и часть из них остаются пустыми
                    Debug.Log("ряд сильно изменился");
                    return _bigChangeY;
                }
            }
            else
            {
                //ряд меняется на дефолтное значение
                Debug.Log("ряд изменился как обычно");
                res = _defaultChangeY;
            }

            emptyRows = 0;
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
                SpawnChunk(_spawnPosition);
            }
        }

        private void OnItemDestroy(ResetSignal<Chunk> signal)
        {
            //сначала SetActive(false), а потом Reset или наоборот? 
            Chunk item = signal.Resetable;
            item.gameObject.SetActive(false);
            item.Reset();
            SpawnChunk(_spawnPosition);
        }
    }
}