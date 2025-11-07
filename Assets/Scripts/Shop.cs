using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tmp;
    [SerializeField] float _activationDistance = 10f;
    [SerializeField] List<UpgradeUI> _upgrades = new List<UpgradeUI>();
    [SerializeField] InputSystem_Actions _playerInputActions;
    [SerializeField] Canvas _shopCanvas;
    [SerializeField] ParticleSystem _disappearParticleSystem;
    [SerializeField] Button _exitButton;

    public Canvas ShopCanvas {get {return _shopCanvas;}}
    bool _isActive { get { return PlayerBoat.Instance.transform.position.DistanceTo(transform.position) < _activationDistance; } }
    bool _isOpened = false;
    bool _purchased = false;
    private void Start()
    {
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += OpenShop;
        foreach(UpgradeUI upgrade in _upgrades)
        {
            upgrade.OnPurchased += () =>
            {
                _purchased = true;
                foreach (UpgradeUI upgrade in _upgrades)
                {
                    upgrade.UpdateState();
                }
            };
        }

    }

    void Update()
    {
        _tmp.transform.localScale = Vector3.Lerp(_tmp.transform.localScale, _isActive ? Vector3.one : Vector3.zero, Time.deltaTime * 10f);
        if (PlayerBoat.Instance)
        {
            float distance = transform.position.DistanceTo(PlayerBoat.Instance.transform.position);
            print(distance);
            if(distance > 200f)
            {
                transform.position = PlayerBoat.Instance.GetRandomProjectedPosition(45f, 100f, 120f);
            }
        }
    }   

    void OpenShop(InputAction.CallbackContext cb)
    {
        if (_isOpened)
        {
            CloseShop();
            return;
        }

        _isOpened = true;
        if (_isActive)
        {
            Game.Instance.IsPlaying = false;
            Game.Instance.SetTargetTimeScale(0.5f);
            foreach(UpgradeUI upgrade in _upgrades)
            {
                upgrade.UpdateState();
            }
            _shopCanvas.gameObject.SetActive(true);
        }
    }

    public void CloseShop()
    {
        _isOpened = false;
        _shopCanvas.gameObject.SetActive(false);
        Game.Instance.IsPlaying = true;
        Game.Instance.SetTargetTimeScale(1.0f);

        if(_purchased)
        {
            Instantiate(_disappearParticleSystem, transform.position, Quaternion.identity);
            transform.position = PlayerBoat.Instance.transform.position + Quaternion.AngleAxis(Random.value * 360f, Vector3.up) * Vector3.left * 200f;
        }

        _purchased = false;
    }
}
