using System;
using Gameplay;
using UnityEngine;
using Zenject;

public class Spring : MonoBehaviour, IResetable, IDespawnable
{
   private const string SPRING_LAYER_NAME = "Spring";
   
   private float _jumpForce;
   private SignalBus _signalBus;
   private int _springLayerNumber;
   
   [Inject]
   public void Construct(float jumpForce, SignalBus signalBus)
   {
      _jumpForce = jumpForce;
      _signalBus = signalBus;
      
      _springLayerNumber = LayerMask.GetMask(SPRING_LAYER_NAME);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.TryGetComponent(out Player player))
      {
         player.SpecialJump(_jumpForce, _springLayerNumber);
      }
   }

   public void Reset()
   {
   }

   public void Despawn()
   {
      _signalBus.Fire(new ResetSignal<Spring>(this));
   }
}
