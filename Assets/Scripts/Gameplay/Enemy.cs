using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
   private int _health;
   private SignalBus _signalBus;
   
   public void TakeDamage(int damage)
   {
      Debug.Log("Получил урон");
      
      _health -= damage;

      if (_health <= 0)
      {
         Die();
      }
   }

   private void Die()
   {
      
   }
}
