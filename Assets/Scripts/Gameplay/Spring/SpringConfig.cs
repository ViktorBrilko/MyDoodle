using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/SpringConfig", fileName = "Configs")]
public class SpringConfig : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _jumpForce;
    public GameObject Prefab => _prefab;
    public float JumpForce => _jumpForce;
}