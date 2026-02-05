using Core.Configs;
using Gameplay.Base;
using Gameplay.Chunks;
using Gameplay.Platforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Boosts
{
    public class BoostGenerationService
    {
        private Spawner<Shield> _shieldSpawner;
        private Spawner<Jetpack> _jetpackSpawner;
        private ChunkConfig _config;

        public BoostGenerationService(Spawner<Shield> shieldSpawner,
            Spawner<Jetpack> jetpackSpawner, ChunkConfig config)
        {
            _shieldSpawner = shieldSpawner;
            _jetpackSpawner = jetpackSpawner;
            _config = config;
        }
        
        public void TrySpawnBoost(BasePlatform basePlatform, ChunkPresentation chunkPresentation)
        {
            if (chunkPresentation.Logic.BoostsInChunk >= _config.MaxBoosts || basePlatform.IsOccupied) return;

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
            chunkPresentation.Logic.Add(boost, boost.transform.localPosition);
            boost.transform.localPosition = basePlatform.ShieldPosition;

            basePlatform.IsOccupied = true;
            chunkPresentation.Logic.BoostsInChunk++;
        }
    }
}