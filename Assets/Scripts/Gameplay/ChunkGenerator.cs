using System;
using Gameplay.Chunks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ChunkGenerator : IInitializable, IDisposable
    {
        private float rightSideOfScreenInWorld =
            Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;

        private float leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;

        private float _stepY;
        private float _chunkHeight;
        private int _changeYChance;
        private int _leaveYChance;
        private int _bigChangeYChance;
        private int _bigChangeY;
        private int _defaultChangeY;

        private int _initialChunksCount;
        private Vector3 _spawnPosition;
        private float _currentY;
        private float _emptyRows;

        private Spawner<Enemy> _enemySpawner;
        private Spawner<Platform> _platformSpawner;
        private ObjectPool<Chunk> _pool;
        private SignalBus _signalBus;
        private Transform _chunkStartPoint;
        private PlayerConfig _playerConfig;

        public ChunkGenerator(ObjectPool<Chunk> pool, PlayerConfig playerConfig, Transform chunkStartPoint, Spawner<Enemy> enemySpawner,
            Spawner<Platform> platformSpawner, ChunkConfig chunkConfig, SignalBus signalBus)
        {
            _enemySpawner = enemySpawner;
            _platformSpawner = platformSpawner;
            _signalBus = signalBus;
            _chunkStartPoint = chunkStartPoint;
            _playerConfig = playerConfig;
            _spawnPosition = _chunkStartPoint.position;
            _pool = pool;

            _stepY = chunkConfig.StepY;
            _chunkHeight = chunkConfig.ChunkHeight;
            _changeYChance = chunkConfig.ChangeYChance;
            _leaveYChance = chunkConfig.LeaveYChance;
            _bigChangeYChance = chunkConfig.BigChangeYChance;
            _initialChunksCount  = chunkConfig.InitialChunksCount;

            _bigChangeY = chunkConfig.BigChangeY;
            _defaultChangeY = chunkConfig.DefaultChangeY;
        }

        private void SpawnChunk(Vector3 position)
        {
            Debug.Log("SpawnChunk");
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
                Vector3 platformPosition = new Vector3(Random.Range(rightSideOfScreenInWorld, leftSideOfScreenInWorld),
                    _currentY, 0);
                Platform platform = _platformSpawner.SpawnItem(platformPosition);
                chunk.Add(platform);

                _currentY += ChangeYRow();
            }

            _currentY = 0;
        }

        private int ChangeYRow()
        {
            int chance = Random.Range(0, 100);
            int res = 0;

            if (_changeYChance <= chance && chance <= _leaveYChance)
            {
                //ряд не меняется
                res = 0;
            }
            else if (_leaveYChance <= chance && chance <= _bigChangeYChance)
            {
                if (_emptyRows < _playerConfig.MaxJumpHeight)
                {
                    _emptyRows += _defaultChangeY;
                    //ряд меняется на несколько и часть из них остаются пустыми
                    return _bigChangeY;
                }
            }
            else
            {
                //ряд меняется на дефолтное значение
                res = _defaultChangeY;
            }

            _emptyRows = 0;
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