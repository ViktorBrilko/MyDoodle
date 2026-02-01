using UnityEngine;

namespace Core
{
    public interface IFabric<TProduct>
    {
        public TProduct Create(Transform parent);
    }
}