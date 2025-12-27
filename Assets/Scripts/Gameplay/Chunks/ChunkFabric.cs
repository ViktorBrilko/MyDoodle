using UnityEngine;
using Zenject;

namespace Gameplay.Chunks
{
    public class ChunkFabric : BaseFabric<Chunk>
    {
        private ChunkConfig _chunkConfig;
        
        public ChunkFabric(ChunkConfig chunkConfig, DiContainer container) : base(container)
        {
            _chunkConfig = chunkConfig;
        }

        public override Chunk Create(Transform parent)
        {
            Chunk chunk = Container.InstantiatePrefabForComponent<Chunk>(_chunkConfig.Prefab, parent,
                new object[] { _chunkConfig.YCameraOffset });

            return chunk;
        }
    }
}