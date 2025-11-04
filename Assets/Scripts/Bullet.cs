using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _force = 1000f;
    [SerializeField] float _verticalForce = 0.01f;
    [SerializeField] WaterSplash _waterSplashPrefab;
    [SerializeField] ParticleSystem _explosionParticlesPrefab;
    Vector3 _targetPosition;
    
    void Start()
    {
        transform.localScale = Vector3.one * 5.0f;
    }
    
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ship ship = collision.collider.GetComponent<Ship>();
        if (ship) {
            ship.TakeDamage(1f);
            Destroy(gameObject);
            Instantiate(_explosionParticlesPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // water
            Instantiate(_waterSplashPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Fire(Vector3 direction)
    {
        _rigidbody.AddForce((direction.normalized + Vector3.up * _verticalForce * direction.magnitude) * _force);
    }
}
