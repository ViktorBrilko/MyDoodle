using UnityEngine;

[CreateAssetMenu(menuName = "GameConfigs/PlayerConfig", fileName = "Configs")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float GravityScale;
    [SerializeField] private float _jumpAidCoef;

    private float _maxJumpHeight;

    public float JumpForce => _jumpForce;

    public float JumpAidCoef => _jumpAidCoef;

    public float Speed => _speed;

    public float MaxJumpHeight => (JumpForce * JumpForce) / (2 * 9.81f * GravityScale) * _jumpAidCoef;
}