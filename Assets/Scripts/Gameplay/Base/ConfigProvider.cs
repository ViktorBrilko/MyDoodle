using Gameplay.Boosts;
using Gameplay.Chunks;
using Gameplay.Platforms;
using UnityEngine;

namespace Gameplay
{
    public class ConfigProvider
    {
        public BulletConfig BulletCfg { get; private set; }
        public EnemyConfig EnemyCfg { get; private set; }
        public PlayerConfig PlayerCfg { get; private set; }
        public PlatformConfig PlatformCfg { get; private set; }
        public SpringConfig SpringCfg { get; private set; }
        public ShieldConfig ShieldCfg { get; private set; }
        public ChunkConfig ChunkCfg { get; private set; }
        public BrokenPlatformConfig BrokenPlatformCfg { get; private set; }
        public JetpackConfig JetpackCfg { get; private set; }
        
        public void LoadAll()
        {
            BulletCfg = LoadFromFile<BulletConfig>("bullet_config.json");
            EnemyCfg = LoadFromFile<EnemyConfig>("enemy_config.json");
            PlayerCfg = LoadFromFile<PlayerConfig>("player_config.json");
            PlatformCfg = LoadFromFile<PlatformConfig>("platform_config.json");
            SpringCfg = LoadFromFile<SpringConfig>("spring_config.json");
            ShieldCfg = LoadFromFile<ShieldConfig>("shield_config.json");
            ChunkCfg = LoadFromFile<ChunkConfig>("chunk_config.json");
            BrokenPlatformCfg = LoadFromFile<BrokenPlatformConfig>("broken_platform_config.json");
            JetpackCfg = LoadFromFile<JetpackConfig>("jetpack_config.json");
        }
        
        private T LoadFromFile<T>(string fileName)
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            return default; 
        }        
    }
}