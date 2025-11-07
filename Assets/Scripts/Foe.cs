using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Foe : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] float _cost = 1f;
    public float Cost { get { return _cost; } }
    [SerializeField] float _coinReward;

    [SerializeField] Ship _ship;
    public Ship Ship { get { return _ship; } }

    [Header("Movement")]
    [SerializeField] float _orbitSpeed = 3f;
    [SerializeField] float _orbitRadius = 5f;
    [SerializeField] float _moveForce = 10f;
    [SerializeField] float _avoidForce = 15f;
    [SerializeField] float _obstacleCheckDistance = 2f;
    [SerializeField] float _sideRayAngle = 30f;
    [SerializeField] LayerMask _obstacleMask;
    float _currentSpeed = 0f;

    [SerializeField] float _maxPlayerDistance = 200f;
    [SerializeField] float _hideDistance = 190f;

    [Header("Fire")]
    [SerializeField] float _fireRate = 2f;
    [SerializeField] float _fireDistance = 8f;
    [SerializeField] int _fireCount = 1; 
    [SerializeField] float _fireBurstSpeed = 0.2f;
    [SerializeField] float _fireSpread = 2.0f;
    float _lastFireTime = 0f;

    [Header("UIs")]
    [SerializeField] Canvas _statsUI;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _timedHpSlider;
    //[SerializeField] Canvas _crosshairPrefab;

    [Header("Loot")]
    [SerializeField] Loot _lootPrefab;
    [SerializeField] LootResource _coinResource;




    private void Start()
    {
        UpdateHp();
        _timedHpSlider.value = _hpSlider.value;
        _ship.OnTakeDamage += UpdateHp;
        _ship.OnDie += DropLoot;
        _currentSpeed = _moveForce;

        Vector3 _orbitDir = Random.value < 0.5 ? Vector3.up : Vector3.down;

        transform.localScale = Vector3.zero;
    }


    private void Update()
    {
        if (PlayerBoat.Instance)
        {
            float distance = transform.position.DistanceTo(PlayerBoat.Instance.transform.position);
            if (distance > _hideDistance)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 5.0f);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5.0f);
            }

            if (distance > _maxPlayerDistance)
            {
                transform.position = PlayerBoat.Instance.GetRandomProjectedPosition(45f, _maxPlayerDistance - 20f, _maxPlayerDistance - 10f);
                transform.localScale = Vector3.zero;
            }

            if (_ship.IsAlive && Game.Instance.IsPlaying)
            {


                OrbitAroundPlayer();

                if ((PlayerBoat.Instance.transform.position - transform.position).magnitude < _fireDistance && Time.time - _lastFireTime > _fireRate)
                {

                    Vector3 firePosition = PlayerBoat.Instance.ProjectedPosition;
                    _lastFireTime = Time.time;
                    StartCoroutine(Fire(firePosition));

                    // firePosition.y = 0.2f;
                    // Canvas ch = Instantiate(_crosshairPrefab, firePosition, Quaternion.identity);
                    // ch.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
                    // bullet.OnDestroy += () => Destroy(ch);
                }
            }
        }


        _statsUI.transform.LookAt(Camera.main.transform.position);
        _timedHpSlider.value = Mathf.Lerp(_timedHpSlider.value, _hpSlider.value, Time.deltaTime);
    }

    IEnumerator Fire(Vector3 position)
    {
        for(int i = 0; i < _fireCount; i++)
        {
            Bullet bullet = _ship.FireBulletTowardPosition(position + Quaternion.AngleAxis(Random.value * 360f, Vector3.up) * Vector3.left * Random.value * _fireSpread);
            _lastFireTime = Time.time;
            yield return new WaitForSeconds(_fireBurstSpeed);
        }

        yield return new WaitForEndOfFrame();
    }

    void FixedUpdate()
    {
        if (!Game.Instance.IsPlaying) return;

       
        float angle = Mathf.Atan2(_ship.Rigidbody.linearVelocity.x, _ship.Rigidbody.linearVelocity.z);
        transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg, 0);

        if (PlayerBoat.Instance)
        {
            float distance = PlayerBoat.Instance.transform.position.DistanceTo(transform.position);
            _moveForce =  distance > 100 ? _currentSpeed + (distance - 100f) / 10f : _currentSpeed;
        }
    }

    void OrbitAroundPlayer()
    {

        // Direction from enemy to player
        Vector3 toPlayer = (PlayerBoat.Instance.transform.position - transform.position);
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        // Orbit direction (perpendicular to player direction)
        Vector3 orbitDir = Vector3.Cross(Vector3.up, toPlayerDir).normalized;

        // Maintain orbit radius
        Vector3 desiredPosition = PlayerBoat.Instance.transform.position - toPlayerDir * _orbitRadius;
        Vector3 moveDir = (desiredPosition - transform.position).normalized + orbitDir * _orbitSpeed;

        // OBSTACLE AVOIDANCE
        Vector3 avoidDir = Vector3.zero;
        bool obstacleDetected = false;

        // Forward ray
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit hitForward, _obstacleCheckDistance, _obstacleMask))
        {
            avoidDir += Vector3.Reflect(moveDir, hitForward.normal);
            obstacleDetected = true;
        }

        // Left and right ray directions
        Quaternion leftRot = Quaternion.AngleAxis(-_sideRayAngle, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(_sideRayAngle, Vector3.up);

        Vector3 move = _ship.Rigidbody.linearVelocity.normalized;
        Vector3 leftDir = leftRot * move;
        Vector3 rightDir = rightRot * move;


        //Debug.DrawLine(transform.position, transform.position + move * _obstacleCheckDistance);

        // Left raycast
        //Debug.DrawLine(transform.position, transform.position + leftDir * _obstacleCheckDistance, Color.green);
        if (Physics.Raycast(transform.position, leftDir, out RaycastHit hitLeft, _obstacleCheckDistance, _obstacleMask))
        {
            avoidDir += Vector3.Reflect(leftDir, hitLeft.normal);
            obstacleDetected = true;
        }

        // Right raycast
        //Debug.DrawLine(transform.position, transform.position + rightDir * _obstacleCheckDistance, Color.red);
        if (Physics.Raycast(transform.position, rightDir, out RaycastHit hitRight, _obstacleCheckDistance, _obstacleMask))
        {
            avoidDir += Vector3.Reflect(rightDir, hitRight.normal);
            obstacleDetected = true;
        }

        // Apply forces
        if (obstacleDetected)
        {
            _ship.TargetVelocity = moveDir * _avoidForce;
        }
        else
        {
            _ship.TargetVelocity = moveDir * _moveForce;
        }
    }

    void UpdateHp()
    {
        _statsUI.gameObject.SetActive(_ship.CurrentHp < _ship.MaxHp && _ship.IsAlive);
        _hpSlider.value = _ship.CurrentHp / _ship.MaxHp;
    }

    void DropLoot()
    {
        Loot loot = Instantiate(_lootPrefab, transform.position, Quaternion.identity);
        loot.LoadRandomResource();
        for(int i = 0; i < _coinReward; i++)
        {
            Loot l = Instantiate(_lootPrefab, transform.position, Quaternion.identity);
            l.LoadResource(_coinResource);
        }
        Game.Instance.EnemyEliminated++;
    }

    
}
