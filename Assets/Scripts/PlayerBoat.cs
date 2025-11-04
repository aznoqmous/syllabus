using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] InputSystem_Actions _playerInputActions;
    [SerializeField] GameObject _cameraContainer;
    [SerializeField] Ship _ship;

    Vector2 _movementInput = new Vector2();
    Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Player.Move.performed += PlayerMove;
        _playerInputActions.Player.Move.Enable();
    }

    void Update()
    {
        _camera.transform.LookAt(transform.position);

        if (Input.GetMouseButtonDown(0)) {
            print("MOUSE BUTTON");
            RaycastHit ray;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, Mathf.Infinity, LayerMask.GetMask("Water")))
            {
                Vector3 pos = ray.point;
                print(pos);
                _ship.FireBullet(pos - transform.position);
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

    void PlayerMove(InputAction.CallbackContext cb)
    {
        _movementInput = cb.ReadValue<Vector2>();
    }
}
