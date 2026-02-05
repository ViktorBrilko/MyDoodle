using System;

namespace Core.Configs
{
    [Serializable]
    public class BulletConfig 
    {
        public int Damage { get; set; } 
        public int Speed { get; set; }
        public float MaxLifetime { get; set; }
    }
}