using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tmp;
    [SerializeField] float _activationDistance = 10f;
    [SerializeField] List<UpgradeUI> _upgrades = new List<UpgradeUI>();
    [SerializeField] InputSystem_Actions _playerInputActions;
    [SerializeField] Canvas _shopCanvas;
    bool _isActive { get { return PlayerBoat.Instance.transform.position.DistanceTo(transform.position) < _activationDistance; } }
    bool _isOpened = false;
    private void Start()
    {
        _playerInputActions = new InputSystem_Actions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += OpenShop;
        foreach(UpgradeUI upgrade in _upgrades)
        {
            upgrade.OnPurchased += () =>
            {
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

    void CloseShop()
    {
        _isOpened = false;
        _shopCanvas.gameObject.SetActive(false);
        Game.Instance.IsPlaying = true;
        Game.Instance.SetTargetTimeScale(1.0f);

        transform.position = PlayerBoat.Instance.transform.position + Quaternion.AngleAxis(Random.value * 360f, Vector3.up) * Vector3.left * 200f;
    }
}
