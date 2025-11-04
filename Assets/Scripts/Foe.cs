using UnityEngine;

public class Foe : MonoBehaviour
{
    [SerializeField] Ship _ship;

    [Header("Movement")]
    [SerializeField] float _orbitSpeed = 3f;
    [SerializeField] float _orbitRadius = 5f;
    [SerializeField] float _moveForce = 10f;
    [SerializeField] float _avoidForce = 15f;
    [SerializeField] float _obstacleCheckDistance = 2f;
    [SerializeField] float _sideRayAngle = 30f;
    [SerializeField] LayerMask _obstacleMask;

    [Header("Fire")]
    [SerializeField] float _fireRate = 2f;
    [SerializeField] float _fireDistance = 8f;
    float _lastFireTime = 0f;

    [SerializeField] Canvas _crosshairPrefab;

    void FixedUpdate()
    {
        if (_ship.IsAlive && PlayerBoat.Instance)
        {
            OrbitAroundPlayer();

            if ((PlayerBoat.Instance.transform.position - transform.position).magnitude < _fireDistance && Time.time - _lastFireTime > _fireRate)
            {

                Vector3 firePosition = PlayerBoat.Instance.ProjectedPosition;
                Bullet bullet = _ship.FireBulletTowardPosition(firePosition);
                _lastFireTime = Time.time;

                firePosition.y = 0.2f;
                Canvas ch = Instantiate(_crosshairPrefab, firePosition, Quaternion.identity);
                ch.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
                bullet.OnDestroy += () => Destroy(ch);
            }
        }
        float angle = Mathf.Atan2(_ship.Rigidbody.linearVelocity.x, _ship.Rigidbody.linearVelocity.z);
        transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg, 0);
    }

    void OrbitAroundPlayer()
    {
        /*Vector3 toPlayer = (PlayerBoat.Instance.transform.position - transform.position);
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        Vector3 orbitDir = Vector3.Cross(Vector3.up, toPlayerDir).normalized;

        Vector3 desiredPosition = PlayerBoat.Instance.transform.position - toPlayerDir * _orbitRadius;
        Vector3 moveDir = (desiredPosition - transform.position).normalized + orbitDir * _orbitSpeed;

        if (Physics.Raycast(transform.position, moveDir, out RaycastHit hit, _obstacleCheckDistance, _obstacleMask))
        {
            Vector3 avoidDir = Vector3.Reflect(moveDir, hit.normal);
            _ship.TargetVelocity = avoidDir * _avoidForce;
        }
        else
        {
            _ship.TargetVelocity = moveDir * _moveForce;
        }*/

        // Direction from enemy to player
        Vector3 toPlayer = (PlayerBoat.Instance.transform.position - transform.position);
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        // Orbit direction (perpendicular to player direction)
        Vector3 orbitDir = Vector3.Cross(Vector3.up, toPlayerDir).normalized;

        // Maintain orbit radius
        Vector3 desiredPosition = PlayerBoat.Instance.transform.position - toPlayerDir * _orbitRadius;
        Vector3 moveDir = (desiredPosition - transform.position).normalized + orbitDir * _orbitSpeed;

        // --- OBSTACLE AVOIDANCE ---
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
        Vector3 leftDir = leftRot * moveDir;
        Vector3 rightDir = rightRot * moveDir;

        // Left raycast
        if (Physics.Raycast(transform.position, leftDir, out RaycastHit hitLeft, _obstacleCheckDistance, _obstacleMask))
        {
            avoidDir += Vector3.Reflect(leftDir, hitLeft.normal);
            obstacleDetected = true;
        }

        // Right raycast
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
}
