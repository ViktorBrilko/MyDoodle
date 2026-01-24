using UnityEngine;
using Zenject;

public class ShieldBoost : MonoBehaviour
{
    private ShieldConfig _config;

    [Inject]
    public void Construct(ShieldConfig config)
    {
        _config = config;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.BecomeInvincible(_config.ShieldInvincibilityTime);
        }
    }
}
