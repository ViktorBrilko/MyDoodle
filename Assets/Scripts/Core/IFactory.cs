using UnityEngine;

namespace Core
{
    public interface IFactory<TProduct>
    where TProduct : Component
    {
        public TProduct Create(Transform parent);
    }
}