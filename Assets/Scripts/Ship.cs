using TMPro;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] public Rigidbody Rigidbody;
    [SerializeField] BoxCollider _collider;
    [SerializeField] float _speed = 3.0f;
    public float Speed { get { return _speed; } set { _speed = value; } }

    [SerializeField] float _acceleration = 0.01f;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] GameObject _bulletContainer;

    [SerializeField] float _bulletSpeed = 100f;

    [SerializeField] float _maxHp = 10f;
    float _currentHp = 10f;
    public float MaxHp { get { return _maxHp; } }
    public float CurrentHp { get { return _currentHp; } }

    [SerializeField] ParticleSystem _damageParticleSystem;
    [SerializeField] ParticleSystem _explosionParticleSystem;
    [SerializeField] ParticleSystem _speedParticles;
    [SerializeField] TrailRenderer _trailRenderer;

    [SerializeField] Animator _shipAnimator;

    [SerializeField] LayerMask _bulletLayerMask;
    Material _trailMaterial;

    public Vector3 TargetVelocity;

    public bool IsAlive { get {
        return _currentHp > 0;
    }}

    public System.Action OnTakeDamage;
    public System.Action OnHeal;
    public System.Action OnDie;

    void Start()
    {
        _currentHp = _maxHp;
        UpdateParticles();

        _trailMaterial = _trailRenderer.material;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5.0f);
    }

    float _animVelocity = 0f;
    private void FixedUpdate()
    {
        if (IsAlive)
        {
            Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, TargetVelocity * _speed, _acceleration);
        }
        var main = _speedParticles.main;
        main.startLifetime = Rigidbody.linearVelocity.magnitude / 20f;

        _animVelocity = Mathf.Lerp(_animVelocity, Rigidbody.linearVelocity.magnitude / 2f, Time.deltaTime * 2f);
        _trailRenderer.time = _animVelocity;
        _trailMaterial.SetFloat("_Length", _animVelocity);
    }

    public Bullet FireBullet(Vector3 direction)
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletContainer.transform.position, Quaternion.identity);
        newBullet.gameObject.layer = Mathf.RoundToInt(Mathf.Log(_bulletLayerMask.value, 2));
        newBullet.Fire(direction);
        return newBullet;
    }

    public Bullet FireBulletTowardPosition(Vector3 targetPosition)
    {
        Vector3 direction = CalculateVelocityToHitTarget(_bulletContainer.transform.position, targetPosition, _bulletSpeed);
        return FireBullet(direction);
    }

    Vector3 CalculateVelocityToHitTarget(Vector3 start, Vector3 target, float speed)
    {
        Vector3 toTarget = target - start;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);

        float distance = toTargetXZ.magnitude;
        float yOffset = toTarget.y;
        float g = Mathf.Abs(Physics.gravity.y);

        float v2 = speed * speed;
        float discriminant = v2 * v2 - g * (g * distance * distance + 2 * yOffset * v2);

        if (discriminant < 0)
        {
            // No valid solution — not enough speed to reach target
            return Vector3.zero;
        }

        float sqrtDisc = Mathf.Sqrt(discriminant);

        // Two possible angles (radians)
        float tanThetaHigh = (v2 + sqrtDisc) / (g * distance);
        float tanThetaLow = (v2 - sqrtDisc) / (g * distance);

        // Choose one — high arc or low arc
        float angle = Mathf.Atan(tanThetaLow); // or tanThetaHigh for a higher shot

        // Build velocity vector
        Vector3 dir = toTargetXZ.normalized;
        Vector3 velocity = dir * speed * Mathf.Cos(angle);
        velocity.y = speed * Mathf.Sin(angle);

        return velocity;
    }

    public void Die()
    {
        _shipAnimator.Play("Death");
        _collider.enabled = false;
        OnDie?.Invoke();
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
        OnTakeDamage?.Invoke();
        if (_currentHp <= 0) Die();
    }

    public void GainHealth(float value)
    {
        _currentHp = Mathf.Clamp(_currentHp + value, 0, _maxHp);
        UpdateParticles();
        OnHeal?.Invoke();
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
