using Core;
using UnityEngine;

namespace Gameplay.Boosts
{
    public abstract class Boost : MonoBehaviour, IDespawnable
    {
        public abstract void Despawn();
    }
}