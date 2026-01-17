using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Zenject;

public class Chunk : MonoBehaviour, IResetable
{
    private SignalBus _signalBus;
    private float _yCameraOffset;
    private List<IDespawnable> _items = new();
    private List<Vector2> _itemsPositions = new();

    public List<Vector2> ItemsPositions => _itemsPositions;

    [Inject]
    public void Construct(float yCameraOffset, SignalBus signalBus)
    {
        _yCameraOffset = yCameraOffset;
        _signalBus = signalBus;
    }

    public void Add(IDespawnable item, Vector2 position)
    {
        _items.Add(item);
        _itemsPositions.Add(position);
    }

    private void Update()
    {
        if (CheckChunkVisibility())
        {       
            TurnOff();
        }
    }

    private bool CheckChunkVisibility()
    {
        return Camera.main.WorldToViewportPoint(transform.position).y <= _yCameraOffset;
    }

    private void TurnOff()
    {
        _signalBus.Fire(new ResetSignal<Chunk>(this));
    }

    public void Reset()
    {
        foreach (var item in _items)
        {
            item.Despawn();
        }

        _items.Clear();
    }
}