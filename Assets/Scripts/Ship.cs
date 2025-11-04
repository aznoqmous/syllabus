using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Ship : MonoBehaviour
{
    [SerializeField] public Rigidbody Rigidbody;
    [SerializeField] float _speed = 3.0f;
    [SerializeField] float _acceleration = 0.01f;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] GameObject _bulletContainer;
    [SerializeField] float _maxHp = 10f;

    [SerializeField] ParticleSystem _damageParticleSystem;
    [SerializeField] ParticleSystem _explosionParticleSystem;

    [SerializeField] Animator _shipAnimator;

    float _currentHp = 10f;

    public Vector3 TargetVelocity;

    void Start()
    {
        _currentHp = _maxHp;
        UpdateParticles();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5.0f);
    }

    private void FixedUpdate()
    {
        Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, TargetVelocity * _speed, _acceleration);
    }

    public void FireBullet(Vector3 direction)
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletContainer.transform.position, Quaternion.identity);
        newBullet.Fire(direction);
    }

    public void Die()
    {
        _shipAnimator.Play("Death");
    }
    public void Erase()
    {
        Destroy(gameObject);
    }

    public void SpawnRandomExplosionParticle()
    {
        Instantiate(_explosionParticleSystem, transform.position + new Vector3(Random.value, Random.value, Random.value), Quaternion.identity);
    }

    public void TakeDamage(float value)
    {
        if (_currentHp <= 0) return;
        _currentHp -= value;
        transform.localScale = Vector3.one * 1.2f;
        UpdateParticles();
        if (_currentHp <= 0) Die();
    }

    public void UpdateParticles()
    {
        float ratio = (1f - _currentHp / _maxHp);
        var startSize = _damageParticleSystem.main.startSize;
        startSize.constantMin = 2f * ratio;
        startSize.constantMax = 3f * ratio;

        var emission = _damageParticleSystem.emission;
        emission.rateOverTime = ratio * 10.0f;
    }
}
