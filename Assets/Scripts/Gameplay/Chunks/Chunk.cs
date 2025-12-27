using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Zenject;

public class Chunk : MonoBehaviour, IResetable
{
    private SignalBus _signalBus;
    private float _yCameraOffset;
    public List<IDespawnable> items = new();


    [Inject]
    public void Construct(float yCameraOffset, SignalBus signalBus)
    {
        _yCameraOffset = yCameraOffset;
        _signalBus = signalBus;
    }

    public void Add(IDespawnable item)
    {
        items.Add(item);
    }

    private void Update()
    {Debug.Log("WorldToViewportPoint " + Camera.main.WorldToViewportPoint(transform.position).y);
        //не сильно ресурсоемко? 
        if (CheckChunkVisibility())
        {
            TurnOff();
        }
    }

    //удали
    private void Start()
    {
        Debug.Log("Camera.main.transform.position.y " + Camera.main.transform.position.y);
        Debug.Log("transform.position.y " + transform.position.y);
        
    }

    private bool CheckChunkVisibility()
    {
        //правильная проверка?
        return Camera.main.WorldToViewportPoint(transform.position).y <= _yCameraOffset;
    }

    private void TurnOff()
    {
        _signalBus.Fire(new ResetSignal<Chunk>(this));
    }

    public void Reset()
    {
        foreach (var item in items)
        {
            item.Despawn();
        }

        items.Clear();
    }
}