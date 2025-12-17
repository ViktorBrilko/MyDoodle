using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/BulletConfig", fileName = "Configs")]
public class BulletConfig : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;
    [SerializeField] private float _maxLifetime;

    public int Damage
    {
        get => _damage;
    }

    public int Speed
    {
        get => _speed;
    }

    public float MaxLifetime
    {
        get => _maxLifetime;
    }
}