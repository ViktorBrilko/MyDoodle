using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/PlatformConfig", fileName = "Configs")]
public class PlatformConfig : ScriptableObject
{
    [SerializeField] private GameObject _prefab;

    public float Width => _prefab.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    public GameObject Prefab => _prefab;
}