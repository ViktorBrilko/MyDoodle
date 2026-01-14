using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/BoostConfig", fileName = "Configs")]
public class BoostConfig : ScriptableObject
{
    [SerializeField] private int _shieldInvincibilityTime;
    
    public int ShieldInvincibilityTime => _shieldInvincibilityTime;
    
}