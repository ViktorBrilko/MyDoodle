using Core.Configs;
using Gameplay.Base;
using Gameplay.Boosts;
using Gameplay.Chunks;
using Gameplay.Springs;
using UnityEngine;

namespace Gameplay.Platforms
{
    public class PlatformGenerationService
    {
        private Spawner<BasePlatform> _platformSpawner;
        private Spawner<BrokenPlatform> _brokenPlatformSpawner;
        private ChunkConfig _config;
        private PositionValidationService _positionValidationService;
        private SpringGenerationService _springGenerationService;
        private BoostGenerationService _boostGenerationService;
        private float _platformWidthHalf;

        public PlatformGenerationService(ChunkConfig config, Spawner<BasePlatform> platformSpawner,
            Spawner<BrokenPlatform> brokenPlatformSpawner,
            PositionValidationService positionValidationService, SpringGenerationService springGenerationService,
            BoostGenerationService boostGenerationService, GameObject platformPrefab)
        {
            _config = config;
            _positionValidationService = positionValidationService;
            _platformSpawner = platformSpawner;
            _brokenPlatformSpawner = brokenPlatformSpawner;
            _springGenerationService = springGenerationService;
            _boostGenerationService = boostGenerationService;
            _platformWidthHalf = platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }

        public void SpawnPlatforms(ChunkPresentation chunkPresentation)
        {
            float currentY = _config.ItemStartYGeneration;
            float lastSpringYPosition = 0;
            int lastYChange = 0;
            bool isLastPlatformBroken = false;

            while (currentY <= _config.ChunkHeight)
            {
                Vector3 candidatePosition =
                    _positionValidationService.GetValidPosition(_platformWidthHalf, currentY, chunkPresentation);

                Platform platform;

                //Определение типа платформы
                int chance = Random.Range(0, 100);
                if (chance < _config.BrokenPlatformChance && lastYChange != _config.BigChangeY &&
                    !isLastPlatformBroken && !Mathf.Approximately(currentY, _config.ItemStartYGeneration))
                {
                    platform = _brokenPlatformSpawner.SpawnItem(candidatePosition);
                    isLastPlatformBroken = true;
                }
                else
                {
                    platform = _platformSpawner.SpawnItem(candidatePosition);
                    isLastPlatformBroken = false;
                }

                platform.transform.SetParent(chunkPresentation.transform);
                platform.transform.localPosition = candidatePosition;
                chunkPresentation.Logic.Add(platform, platform.transform.localPosition);

                BasePlatform basePlatform = platform as BasePlatform;
                if (basePlatform != null)
                {
                    lastSpringYPosition = _springGenerationService.TrySpawnSpring(currentY, lastSpringYPosition,
                        basePlatform, chunkPresentation);
                    _boostGenerationService.TrySpawnBoost(basePlatform, chunkPresentation);
                }

                lastYChange = ChangeYRow(isLastPlatformBroken);
                currentY += lastYChange;
            }
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
    }
}