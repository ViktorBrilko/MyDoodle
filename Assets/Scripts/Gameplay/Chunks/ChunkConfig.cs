using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Chunks
{
    [CreateAssetMenu(menuName = "GameConfigs/ChunkConfig", fileName = "Configs")]
    public class ChunkConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _chunkHeight;
        [SerializeField] private int _changeYChance;
        [SerializeField] private int _leaveYChance;
        [SerializeField] private int _bigChangeYChance;
        [SerializeField] private int _bigChangeY;
        [SerializeField] private int _defaultChangeY;
        [SerializeField] private float _yCameraOffset;
        [SerializeField] private int _initialChunksCount;

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