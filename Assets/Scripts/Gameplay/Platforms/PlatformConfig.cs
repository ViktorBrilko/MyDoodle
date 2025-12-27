using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/PlatformConfig", fileName = "Configs")]
public class PlatformConfig : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    public GameObject Prefab => _prefab;
}