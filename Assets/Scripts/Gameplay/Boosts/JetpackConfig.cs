using System;

namespace Gameplay.Boosts
{
    [Serializable]
    public class JetpackConfig
    {
        public int Speed { get; set; }
        public int FlightTime { get; set; }
    }
}