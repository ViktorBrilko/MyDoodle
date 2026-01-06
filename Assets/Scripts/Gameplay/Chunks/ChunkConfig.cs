using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Chunks
{
    [CreateAssetMenu(menuName = "GameConfigs/ChunkConfig", fileName = "Configs")]
    public class ChunkConfig : ScriptableObject
    {
        [Header("Main")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _chunkHeight;
        [SerializeField] private float _yCameraOffset;
        [SerializeField] private int _initialChunksCount;
        [SerializeField] private int _maxPositionAttempts;
       
        [Header("Platforms")]
        [SerializeField] private int _changeYChance;
        [SerializeField] private int _leaveYChance;
        [SerializeField] private int _bigChangeYChance;
        [SerializeField] private int _bigChangeY;
        [SerializeField] private int _defaultChangeY;
        [SerializeField] private float _platformXDistanceCoef;
        [SerializeField] private int _platformStartYGeneration;
        
        [Header("Enemies")]
        [SerializeField] private int _maxEnemiesInChunk;
        [SerializeField] private int _minEnemiesInChunk;
        [SerializeField] private int _enemyStartYGeneration;

        public int MinEnemiesInChunk => _minEnemiesInChunk;
        public int EnemyStartYGeneration => _enemyStartYGeneration;
        public int PlatformStartYGeneration => _platformStartYGeneration;
        public int MaxEnemiesInChunk => _maxEnemiesInChunk;
        public float PlatformXDistanceCoef => _platformXDistanceCoef;
        public int MaxPositionAttempts => _maxPositionAttempts;
        public int InitialChunksCount => _initialChunksCount;
        public float YCameraOffset => _yCameraOffset;
        public float ChunkHeight => _chunkHeight;
        public int ChangeYChance => _changeYChance;
        public int LeaveYChance => _leaveYChance;
        public int BigChangeYChance => _bigChangeYChance;
        public int BigChangeY => _bigChangeY;
        public int DefaultChangeY => _defaultChangeY;
        public GameObject Prefab => _prefab;
    }
}