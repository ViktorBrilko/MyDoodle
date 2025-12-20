using Gameplay;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private Collider2D _groundChecker;
    [SerializeField] private Transform _bulletSpawnPoint;

    private BaseSpawner<Bullet> _bulletSpawner;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    [Inject]
    public void Construct(BaseSpawner<Bullet> spawner)
    {
        _bulletSpawner = spawner;
    }

    public void Move(float direction)
    {
        transform.Translate(new Vector3(direction, 0, 0) * _speed * Time.deltaTime);
    }

    public void Fire()
    {
        _bulletSpawner.SpawnItem(_bulletSpawnPoint);
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
            velocity.y = _jumpForce;
            _rigidbody2D.velocity = velocity;
        }
    }
}