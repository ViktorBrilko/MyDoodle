using UnityEngine;
using Zenject;

namespace Gameplay.Chunks
{
    public class ChunkFabric : BaseFabric<Chunk>
    {
        private ChunkConfig _chunkConfig;
        private GameObject _prefab;
        
        public ChunkFabric(ChunkConfig chunkConfig, DiContainer container, GameObject prefab) : base(container)
        {
            _chunkConfig = chunkConfig;
            _prefab = prefab;
        }

        public override Chunk Create(Transform parent)
        {
            Chunk chunk = Container.InstantiatePrefabForComponent<Chunk>(_prefab, parent,
                new object[] { _chunkConfig.YCameraOffset });

            return chunk;
        }
    }
}