using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/EnemyConfig", fileName = "Configs")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _score;

    public int Score => _score;
    public int Health => _health;
    public GameObject Prefab => _prefab;
    public float Width => _prefab.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
}