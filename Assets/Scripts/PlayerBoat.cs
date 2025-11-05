using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] InputSystem_Actions _playerInputActions;
    [SerializeField] GameObject _cameraContainer;
    [SerializeField] Ship _ship;

    Vector2 _movementInput = new Vector2();
    Camera _camera;

    public static PlayerBoat Instance;

    public Vector3 ProjectedPosition { get { return transform.position + _ship.Rigidbody.linearVelocity; } }

    public float MaxHp { get { return _ship.MaxHp;  } }
    public float CurrentHp { get { return _ship.CurrentHp; } }

    public int _maxBullets = 10;
    public int _currentBullets = 10;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _camera = Camera.main;
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Player.Move.performed += PlayerMove;
        _playerInputActions.Player.Enable();

        _ship.OnTakeDamage += StatsUI.Instance.UpdatePlayerHP;
        _ship.OnHeal += StatsUI.Instance.UpdatePlayerHP;
    }

    void Update()
    {
        _camera.transform.LookAt(transform.position);

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit ray;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, Mathf.Infinity, LayerMask.GetMask("Water")))
            {
                Vector3 pos = ray.point;
                //_ship.FireBullet(pos - transform.position);
                _ship.FireBulletTowardPosition(pos);
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

        //Quaternion.Slerp()
    }

    private void OnCollisionEnter(Collision collision)
    {
        Loot loot = collision.collider.GetComponent<Loot>();
        if (loot)
        {
            print(loot);
            switch (loot.Resource.Effect)
            {
                case LootEffect.RestoreBullet:
                    _currentBullets = Mathf.Clamp(_currentBullets + (int) loot.Resource.Value, 0, _maxBullets);
                    break;
                case LootEffect.RestoreHealth:
                    _ship.GainHealth(loot.Resource.Value);
                    break;
            }
            Destroy(loot.gameObject);
        }
    }

    void PlayerMove(InputAction.CallbackContext cb)
    {
        _movementInput = cb.ReadValue<Vector2>();
    }
}
