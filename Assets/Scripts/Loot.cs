using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Loot : MonoBehaviour
{

    [SerializeField] List<LootResource> _lootResources;
    [SerializeField] Transform _modelContainer;
    [SerializeField] Animator _animator;
    [SerializeField] Collider _collider;
    [SerializeField] Canvas _canvas;
    [SerializeField] TextMeshProUGUI _tmp;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] AudioSource _lootedAudio;

    float _lifeTime = 0f;
    [SerializeField] float _maxLifeTime = 60f;

    LootResource _resource;
    [SerializeField] LootResource _healthResource;
    public LootResource Resource { get { return _resource;  } }

    private void Start()
    {
        _rigidbody.AddForce(Quaternion.AngleAxis(Random.value * 360, Vector3.up) *  Vector3.left * 100f);
    }

    private void Update()
    {
        _lifeTime += Time.deltaTime;
        if(_lifeTime > _maxLifeTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 5f);
        }
        if (transform.localScale.magnitude < 0.1f) {
            Destroy(gameObject);
        }
        _canvas.transform.LookAt(Camera.main.transform);
    }

    private void FixedUpdate()
    {
        if(transform.position.DistanceTo(PlayerBoat.Instance.transform.position) < PlayerBoat.Instance.AttractionDistance)
        {
            _rigidbody.linearVelocity += (PlayerBoat.Instance.transform.position - transform.position).normalized * 10f;
        }
    }
    public void LoadRandomResource()
    {
        if(Random.value > PlayerBoat.Instance.CurrentHp / PlayerBoat.Instance.MaxHp)
        {
            LoadResource(_healthResource);
        }
        else
        {
            LoadResource(_lootResources.PickRandom());
        }
    }

    public void LoadResource(LootResource resource)
    {
        foreach (Transform t in _modelContainer) Destroy(t.gameObject);
        Instantiate(resource.Model, _modelContainer);
        _resource = resource;
        _tmp.text = resource.Text;
        _lootedAudio.clip = resource.LootedAudioClip;
    }

    public void Erase()
    {
        Destroy(gameObject);
    }

    public void PlayLootedAnim()
    {
        _lootedAudio.PlayAtRandomPitch(0.2f);
        _collider.enabled = false;
        _animator.Play("LootLootedAnim");
        _lifeTime = 0;
    }
}
