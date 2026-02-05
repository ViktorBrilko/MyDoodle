using Core.Configs;
using Gameplay.Base;
using Gameplay.Chunks;
using Gameplay.Platforms;
using UnityEngine;

namespace Gameplay.Springs
{
    public class SpringGenerationService
    {
        private Spawner<Spring> _springSpawner;
        private ChunkConfig _config;

        public SpringGenerationService(ChunkConfig config, Spawner<Spring> springSpawner)
        {
            _config = config;
            _springSpawner = springSpawner;
        }

        public float TrySpawnSpring(float currentY, float lastSpringYPosition, BasePlatform platform,
            ChunkPresentation chunkPresentation)
        {
            if (chunkPresentation.Logic.SpringsInChunk >= chunkPresentation.Logic.MaxSpringsInChunk ||
                platform.IsOccupied) return lastSpringYPosition;

            if (currentY - lastSpringYPosition < _config.MinYDistanceBetweenSprings) return lastSpringYPosition;

            int chance = Random.Range(0, 100);
            if (chance > _config.SpringedPlatformChance) return lastSpringYPosition;

            Spring spring = _springSpawner.SpawnItem(new Vector3());
            spring.transform.SetParent(platform.transform);
            platform.Add(spring);
            spring.transform.localPosition = platform.SpringPosition;
            platform.IsOccupied = true;
            chunkPresentation.Logic.SpringsInChunk++;

            return currentY;
        }
    }
}