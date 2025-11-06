using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoat : MonoBehaviour
{
    public static PlayerBoat Instance;

    [SerializeField] InputSystem_Actions _playerInputActions;
    Vector2 _movementInput = new Vector2();

    [Header("World")]
    [SerializeField] GameObject _cameraContainer;
    Camera _camera;
    [SerializeField] MeshRenderer _water;
    public Vector3 ProjectedPosition { get { return transform.position + _ship.Rigidbody.linearVelocity; } }

    [Header("Ship")]
    [SerializeField] Ship _ship;
    public Ship Ship { get { return _ship; } }
    public float MaxHp { get { return _ship.MaxHp;  } }
    public float CurrentHp { get { return _ship.CurrentHp; } }

    [SerializeField] float _reloadTime = 1f;
    float _currentReloadTime = 0f;
    [SerializeField] StatsUI _statsCanvas;
    [SerializeField] BulletsUI _bulletCanvas;
    int _currentBullets;
    public int CurrentBullets { get { return _currentBullets; }}
    [SerializeField] int _maxBullets = 3;
    public int MaxBullets { get { return _maxBullets; }}

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _currentBullets = _maxBullets;
        _bulletCanvas.UpdatePlayerBullets();

        _camera = Camera.main;
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Player.Move.performed += PlayerMove;
        _playerInputActions.Player.Enable();

        _ship.OnTakeDamage += _statsCanvas.UpdatePlayerHP;
        _ship.OnHeal += _statsCanvas.UpdatePlayerHP;
    }

    void Update()
    {
        _water.transform.position = transform.position;

        if (!Game.Instance.IsPlaying) return;

        if (Input.GetMouseButtonDown(0) && _currentBullets > 0) {
            _currentBullets--;
            _bulletCanvas.UpdatePlayerBullets();
            RaycastHit ray;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, Mathf.Infinity, LayerMask.GetMask("Water")))
            {
                MouseUI.Instance.CrossHairImage.transform.localScale  = Vector3.one * 1.5f;
                Vector3 pos = ray.point;
                //_ship.FireBullet(pos - transform.position);
                _ship.FireBulletTowardPosition(pos);
            }
        }


        if (_currentBullets < _maxBullets) {
            _currentReloadTime += Time.deltaTime;
            if (_currentReloadTime > _reloadTime) {
                _currentBullets++;
                _bulletCanvas.UpdatePlayerBullets();
                _currentReloadTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        _movementInput = _playerInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 velocity = new Vector3(_movementInput.x, 0, _movementInput.y);
        _ship.TargetVelocity = velocity;

        float angle = Mathf.Atan2(_ship.Rigidbody.linearVelocity.x, _ship.Rigidbody.linearVelocity.z);
        _ship.Rigidbody.transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg, 0);
        _cameraContainer.transform.position = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Loot loot = collision.collider.GetComponent<Loot>();
        if (loot)
        {
            loot.PlayLootedAnim();
            switch (loot.Resource.Effect)
            {
                case LootEffect.RestoreBullet:
                    _currentBullets = Mathf.Clamp(_currentBullets + (int) loot.Resource.Value, 0, _maxBullets);
                    break;
                case LootEffect.RestoreHealth:
                    _ship.GainHealth(loot.Resource.Value);
                    break;
                case LootEffect.GainCoins:
                    GainCoins((int)loot.Resource.Value);
                    break;
            }
        }
    }

    void PlayerMove(InputAction.CallbackContext cb)
    {
        _movementInput = cb.ReadValue<Vector2>();
    }

    public Vector3 GetRandomProjectedPosition(float angle, float minDistance, float maxDistance)
    {
        return transform.position + (Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.up) * Ship.Rigidbody.linearVelocity.normalized) * Random.Range(minDistance, maxDistance);
    }

    [SerializeField] int _coins = 0;
    public int Coins { get { return _coins; } }
    public void GainCoins(int amount)
    {
        _coins += amount;
    }
    public void LoseCoins(int amount)
    {
        _coins -= amount;
    }

    float _attractionDistance = 10;
    public float AttractionDistance { get { return _attractionDistance; } }
    public void AddUpgrade(UpgradeResource upgrade)
    {
        switch (upgrade.Type)
        {
            case UpgradeType.MaxBullets:
                _maxBullets++;
                _bulletCanvas.UpdateMaxBullets();
                _bulletCanvas.UpdatePlayerBullets();
                break;
            case UpgradeType.MaxHp:
                _ship.MaxHp++;
                _statsCanvas.UpdatePlayerMaxHp();
                _statsCanvas.UpdatePlayerHP();
                break;
            case UpgradeType.Speed:
                _ship.Speed += 10;
                break;
            case UpgradeType.AttractionDistance:
                _attractionDistance *= 1.5f;
                break;
            default:
                break;
        }
    }
}
