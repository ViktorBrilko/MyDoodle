using Gameplay;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private PlayerConfig _config;
    [SerializeField] private Collider2D _groundChecker;
    [SerializeField] private Transform _bulletSpawnPoint;

    private Spawner<Bullet> _bulletSpawner;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    [Inject]
    public void Construct(PlayerConfig config, Spawner<Bullet> spawner)
    {
        _config = config;
        _bulletSpawner = spawner;
    }

    public void Move(float direction)
    {
        transform.Translate(new Vector3(direction, 0, 0) * _config.Speed * Time.deltaTime);
    }

    public void Fire()
    {
        _bulletSpawner.SpawnItem(_bulletSpawnPoint.position);
    }

    private bool IsGrounded()
    {
        return _groundChecker.IsTouchingLayers(LayerMask.GetMask("Platform"))
               || _groundChecker.IsTouchingLayers(LayerMask.GetMask("Enemy"));
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.y = _config.JumpForce;
            _rigidbody2D.velocity = velocity;
        }
    }
}