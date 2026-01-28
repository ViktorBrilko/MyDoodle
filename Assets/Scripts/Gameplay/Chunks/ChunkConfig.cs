using System;

namespace Gameplay.Chunks
{
    [Serializable]
    public class ChunkConfig 
    {
        public float ChunkHeight { get; set; }
        public float YCameraOffset { get; set; }
        public int InitialChunksCount { get; set; }
        public int MaxPositionAttempts { get; set; }
        public int ItemStartYGeneration { get; set; }

        public int LeaveYChance { get; set; }
        public int BigChangeYChance { get; set; }
        public int BigChangeY { get; set; }
        public int DefaultChangeY { get; set; }
        public float PlatformXDistanceCoef { get; set; }

        public int MaxEnemiesInChunk { get; set; }
        public int MinEnemiesInChunk { get; }
        public int MaxYDistanceBetweenEnemies { get; set; }
        public int MinYDistanceBetweenEnemies { get; set; }

        public int MaxSprings { get; set; }
        public int MinSprings { get; set; }
        public int SpringedPlatformChance { get; set; }
        public int MinYDistanceBetweenSprings { get; set; }
        
        public int MaxBoosts { get; set; }
        public int BoostedPlatformChance { get; set; }
        public int ShieldChance { get; set; }
        public int JetpackChance { get; set; }
        
        public int BrokenPlatformChance { get; set; }
    }
}