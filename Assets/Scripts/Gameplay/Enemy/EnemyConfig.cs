using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/EnemyConfig", fileName = "Configs")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _prefab;

    public int Health => _health;
    public GameObject Prefab => _prefab;
}