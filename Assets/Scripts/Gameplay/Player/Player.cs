using System.Collections;
using Gameplay;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D _groundChecker;
    [SerializeField] private Transform _bulletSpawnPoint;

    private PlayerConfig _playerConfig;
    private Spawner<Bullet> _bulletSpawner;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private bool _isAlive = true;
    private bool _isInvincible;
    private SignalBus _signalBus;

    public bool IsInvincible => _isInvincible;
    public Collider2D GroundChecker => _groundChecker;
    public Rigidbody2D Rigidbody => _rigidbody;
    public bool IsAlive => _isAlive;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    [Inject]
    public void Construct(SignalBus signalBus, PlayerConfig config, Spawner<Bullet> spawner)
    {
        transform.SetParent(null);
        _playerConfig = config;
        _bulletSpawner = spawner;
        _signalBus = signalBus;
    }

    public void Move(float direction)
    {
        transform.Translate(new Vector3(direction, 0, 0) * _playerConfig.Speed * Time.deltaTime);
    }

    public void BecomeInvincible(int invincibilityTime)
    {
        StartCoroutine(InvincibilityRoutine(invincibilityTime));
    }

    private IEnumerator InvincibilityRoutine(int invincibilityTime)
    {
        _isInvincible = true;
        Color regularColor = _renderer.color;
        _renderer.color = Color.magenta;
        yield return new WaitForSeconds(invincibilityTime);
        _renderer.color = regularColor;
        _isInvincible = false;
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
        _signalBus.Fire(new PlayerDiedSignal());
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
            velocity.y = _playerConfig.JumpForce;
            _rigidbody.velocity = velocity;
        }
    }
}