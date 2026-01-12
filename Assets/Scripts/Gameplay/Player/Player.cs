using Gameplay;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D _groundChecker;
    [SerializeField] private Transform _bulletSpawnPoint;

    private PlayerConfig _config;
    private Spawner<Bullet> _bulletSpawner;
    private Rigidbody2D _rigidbody;
    private bool _isAlive = true;
    public Collider2D GroundChecker => _groundChecker;
    public Rigidbody2D Rigidbody => _rigidbody;
    public bool IsAlive => _isAlive;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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

    public void Die()
    {
        GetComponent<PlayerInputHandler>().enabled = false;
        _groundChecker.enabled = false;
        _isAlive = false;
    }

    private bool IsGrounded()
    {
        return _groundChecker.IsTouchingLayers(LayerMask.GetMask("Platform"))
               || _groundChecker.IsTouchingLayers(LayerMask.GetMask("Enemy"));
    }

    public void Jump()
    {
        if (IsGrounded() && _rigidbody.velocity.y <= 0)
        {
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = _config.JumpForce;
            _rigidbody.velocity = velocity;
        }
    }
}