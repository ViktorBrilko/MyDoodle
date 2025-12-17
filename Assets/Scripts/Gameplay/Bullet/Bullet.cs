using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IResetable
{
    private int _damage;
    private int _speed;
    private float _maxLifetime;
    private SignalBus _signalBus;
    
    public float CurrentLifetime { get; set; }

    private void Update()
    {
        Move();
        CheckLifetime();
    }

    private void CheckLifetime()
    {
        CurrentLifetime += Time.deltaTime;
        if (CurrentLifetime >= _maxLifetime)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
            DestroyBullet();
        }
    }

    [Inject]
    public void Construct(int speed, int damage, float maxLifetime, SignalBus signalBus)
    {
        _speed = speed;
        _damage = damage;
        _maxLifetime = maxLifetime;
        _signalBus = signalBus;
    }

    private void Move()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        _signalBus.Fire(new ResetSignal<Bullet>(this));
    }
}