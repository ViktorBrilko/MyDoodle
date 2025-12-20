using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/BulletConfig", fileName = "Configs")]
public class BulletConfig : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;
    [SerializeField] private float _maxLifetime;
    [SerializeField] private GameObject _prefab;

    public GameObject Prefab => _prefab;
    public int Damage => _damage;
    public int Speed => _speed;
    public float MaxLifetime => _maxLifetime;

   
}