using UnityEngine;

public interface IFabric<T>
{
    public T Create(Transform parent);
}