using UnityEngine;

namespace Gameplay
{
    public interface IFabric<TProduct>
    {
        public TProduct Create(Transform parent);
    }
}