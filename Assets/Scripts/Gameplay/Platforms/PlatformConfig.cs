using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/PlatformConfig", fileName = "Configs")]
public class PlatformConfig : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _springPosition;

    public float Width => _prefab.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    public GameObject Prefab => _prefab;
    public Vector3 SpringPosition => _springPosition;
}