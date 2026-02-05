using System;
using Core;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Chunks
{
    public class ChunkPresentation : MonoBehaviour, IResetable
    {
        private SignalBus _signalBus;
        public ChunkLogic Logic { get; private set; }

        [Inject]
        public void Construct(ChunkLogic chunkLogic, SignalBus signalBus)
        {
            Logic = chunkLogic;
            _signalBus = signalBus;
        }
        
        private void Update()
        {
            if (CheckChunkVisibility())
            {
                TurnOff();
            }
        }
        
        public void Reset()
        {
            Logic.Reset();
        }

        private void TurnOff()
        {
            _signalBus.Fire(new ResetSignal<ChunkPresentation>(this));
        }

        private bool CheckChunkVisibility()
        {
            return Camera.main.WorldToViewportPoint(transform.position).y <= Logic.YCameraOffset;
        }
    }
}