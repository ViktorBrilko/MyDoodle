using System.Collections;
using Core.Configs;
using Gameplay.Boosts;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private const string PLATFORM_LAYER_NAME = "Platform";
        private const string ENEMY_LAYER_NAME = "Enemy";

        [SerializeField] private Player _player;
        [SerializeField] private Collider2D _groundChecker;

        private SignalBus _signalBus;
        private int _platfromLayerNumber;
        private int _enemyLayerNumber;
        private PlayerConfig _playerConfig;
        private Rigidbody2D _rigidbody2D;

        public Player Player => _player;

        public Collider2D GroundChecker => _groundChecker;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        [Inject]
        public void Construct(SignalBus signalBus, PlayerConfig config)
        {
            _playerConfig = config;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _platfromLayerNumber = LayerMask.GetMask(PLATFORM_LAYER_NAME);
            _enemyLayerNumber = LayerMask.GetMask(ENEMY_LAYER_NAME);
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rigidbody2D.gravityScale = _playerConfig.GravityScale;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(TestRoutine());
            }
        }

        private IEnumerator TestRoutine()
        {
            float elapsedTime = 0;
            _player.BecomeInvincible(10);
            Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            while (elapsedTime < 10)
            {
                transform.Translate(Vector3.up * 10 * Time.deltaTime);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
#endif

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerGetJetpackSignal>(OnTakingJetpack);
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDeath);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerGetJetpackSignal>(OnTakingJetpack);
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDeath);
        }

        public void Move(float direction)
        {
            transform.Translate(new Vector3(direction, 0, 0) * _playerConfig.Speed * Time.deltaTime);
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

        private void OnTakingJetpack(PlayerGetJetpackSignal signal)
        {
            StopCoroutine(FlyingRoutine(signal.Jetpack));
            ResetFlying();
            StartCoroutine(FlyingRoutine(signal.Jetpack));
        }

        private void OnPlayerDeath()
        {
            _groundChecker.enabled = false;
        }

        private void ResetFlying()
        {
            _player.ResetInvincibility();
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private IEnumerator FlyingRoutine(Jetpack jetpack)
        {
            float elapsedTime = 0;
            _player.BecomeInvincible(jetpack.FlightTime);
            Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            while (elapsedTime < jetpack.FlightTime)
            {
                transform.Translate(Vector3.up * jetpack.Speed * Time.deltaTime);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private bool IsGrounded()
        {
            return _groundChecker.IsTouchingLayers(_platfromLayerNumber)
                   || _groundChecker.IsTouchingLayers(_enemyLayerNumber);
        }
    }
}