using System;
using UnityEngine;

namespace Gameplay.Platforms
{
    [Serializable]
    public class PlatformConfig 
    {
        public Vector3 SpringPosition { get; set; } 
        public Vector3 BoostPosition { get; set; } 
    }
}