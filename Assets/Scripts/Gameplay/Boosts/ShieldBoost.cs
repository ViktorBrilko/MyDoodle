using UnityEngine;
using Zenject;

public class ShieldBoost : MonoBehaviour
{
    private int _shieldInvincibilityTime;

    [Inject]
    public void Construct(ShieldConfig config)
    {
        _shieldInvincibilityTime = config.ShieldInvincibilityTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.BecomeInvincible(_shieldInvincibilityTime);
        }
    }
}
