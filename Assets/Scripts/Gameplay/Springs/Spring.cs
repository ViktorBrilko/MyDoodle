using Core;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Springs
{
   public class Spring : MonoBehaviour, IResetable, IDespawnable
   {
      private const string SPRING_LAYER_NAME = "Spring";
   
      private float _jumpForce;
      private SignalBus _signalBus;
      private int _springLayerNumber;
   
      [Inject]
      public void Construct(SpringConfig config, SignalBus signalBus)
      {
         _jumpForce = config.JumpForce;
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
}
