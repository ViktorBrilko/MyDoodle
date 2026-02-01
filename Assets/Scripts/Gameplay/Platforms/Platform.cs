using Core;
using UnityEngine;

namespace Gameplay.Platforms
{
    public abstract class Platform : MonoBehaviour, IDespawnable, IResetable
    {
        public virtual void Despawn()
        {
        }

        public virtual void Reset()
        {
           
        }
    }
}