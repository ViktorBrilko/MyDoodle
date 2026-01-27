using UnityEngine;

namespace Gameplay.Platforms
{
    public abstract class Platform : MonoBehaviour, IDespawnable
    {
        public virtual void Despawn()
        {
        }
    }
}