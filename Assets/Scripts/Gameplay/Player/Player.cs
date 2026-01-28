using System.Collections;
using Gameplay;
using Gameplay.Boosts;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
{
    private const string PLATFORM_LAYER_NAME = "Platform";
    private const string ENEMY_LAYER_NAME = "Enemy";

    [SerializeField] private Collider2D _groundChecker;
    [SerializeField] private Transform _bulletSpawnPoint;

    private PlayerConfig _playerConfig;
    private Spawner<Bullet> _bulletSpawner;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private bool _isAlive = true;
    private bool _isInvincible;
    private SignalBus _signalBus;
    private int _platfromLayerNumber;
    private int _enemyLayerNumber;

    public bool IsInvincible => _isInvincible;
    public Collider2D GroundChecker => _groundChecker;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public bool IsAlive => _isAlive;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();

        _platfromLayerNumber = LayerMask.GetMask(PLATFORM_LAYER_NAME);
        _enemyLayerNumber = LayerMask.GetMask(ENEMY_LAYER_NAME);
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<PlayerGetJetpackSignal>(OnTakingJetpack);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PlayerGetJetpackSignal>(OnTakingJetpack);
    }

    private void Start()
    {
        _rigidbody2D.gravityScale = _playerConfig.GravityScale;
    }

    [Inject]
    public void Construct(SignalBus signalBus, PlayerConfig config, Spawner<Bullet> spawner)
    {
        transform.SetParent(null);
        _playerConfig = config;
        _bulletSpawner = spawner;
        _signalBus = signalBus;
    }

    private void OnTakingJetpack(PlayerGetJetpackSignal signal)
    {
        StartCoroutine(FlyingRoutine(signal.Jetpack));
    }

    private IEnumerator FlyingRoutine(Jetpack jetpack)
    {
        float elapsedTime = 0;
        BecomeInvincible(jetpack.FlightTime);
        Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        while (elapsedTime < jetpack.FlightTime)
        {
            transform.Translate(Vector3.up * jetpack.Speed * Time.deltaTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Move(float direction)
    {
        transform.Translate(new Vector3(direction, 0, 0) * _playerConfig.Speed * Time.deltaTime);
    }

    public void BecomeInvincible(float invincibilityTime)
    {
        StopCoroutine(InvincibilityRoutine(invincibilityTime));
        StartCoroutine(InvincibilityRoutine(invincibilityTime));
    }

    private IEnumerator InvincibilityRoutine(float invincibilityTime)
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
        _isInvincible = false;
        _signalBus.Fire(new PlayerDiedSignal());
    }

    private bool IsGrounded()
    {
        return _groundChecker.IsTouchingLayers(_platfromLayerNumber)
               || _groundChecker.IsTouchingLayers(_enemyLayerNumber);
    }

    public void Jump()
    {
        if (IsGrounded() && _rigidbody2D.velocity.y <= 0)
        {
            ChangePlayerVelocity(_playerConfig.JumpForce, 0);
        }
    }

    public void ChangePlayerVelocity(float yVelocity, float xVelocity)
    {
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.y = yVelocity;
        velocity.x = xVelocity;
        _rigidbody2D.velocity = velocity;
    }

    public void SpecialJump(float jumpForce, int layerMask)
    {
        bool groundedOnLayer = _groundChecker.IsTouchingLayers(layerMask);

        if (groundedOnLayer && _rigidbody2D.velocity.y <= 0)
        {
            ChangePlayerVelocity(jumpForce, 0);
        }
    }
}