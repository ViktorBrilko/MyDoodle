using Core;
using UnityEngine;

namespace Gameplay.Boosts
{
    public class Boost : MonoBehaviour, IDespawnable, IResetable
    {
        public virtual void Despawn()
        {
            
        }

        public void Reset()
        {
            
        }
    }
}