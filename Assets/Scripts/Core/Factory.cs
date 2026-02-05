using UnityEngine;
using Zenject;

namespace Core
{
    public class Factory<TProduct> : IFactory<TProduct>
        where TProduct : Component
    {
        private DiContainer _container;
        private GameObject _prefab;


        [Inject]
        public Factory(DiContainer container, GameObject prefab)
        {
            _container = container;
            _prefab = prefab;
        }

        public TProduct Create(Transform parent)
        {
            TProduct product = _container.InstantiatePrefabForComponent<TProduct>(_prefab, parent);

            return product;
        }
    }
}