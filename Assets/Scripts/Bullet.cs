using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _force = 1000f;
    [SerializeField] float _verticalForce = 0.01f;
    [SerializeField] WaterSplash _waterSplashPrefab;
    [SerializeField] ParticleSystem _explosionParticlesPrefab;
    [SerializeField] ParticleSystem _smokeParticles;
    [SerializeField] AudioSource _explosionAudio;
    
    Vector3 _targetPosition;

    public System.Action OnDestroy;

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
        }

        MeshCollider water = collision.collider.GetComponent<MeshCollider>();
        if(water) Instantiate(_waterSplashPrefab, transform.position, Quaternion.identity);
        else
        {
            Instantiate(_explosionParticlesPrefab, transform.position, Quaternion.identity);
            _explosionAudio.transform.parent = null;
            _explosionAudio.pitch = Random.Range(0.9f, 1.1f);
            _explosionAudio.Play();
            Destroy(_explosionAudio.gameObject, 1f);
        }

        _smokeParticles.EnableEmission(false);
        var main = _smokeParticles.main;
        main.loop = false;
        _smokeParticles.transform.parent = null;

        OnDestroy?.Invoke();    
        Destroy(gameObject);
    }

    public void Fire(Vector3 direction)
    {
        _rigidbody.AddForce((direction.normalized + Vector3.up * _verticalForce * direction.magnitude) * _force);

    }

    
}
