using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] UpgradeResource _upgradeResource;
    [SerializeField] TextMeshProUGUI _upgradeName;
    [SerializeField] TextMeshProUGUI _upgradeLevel;
    [SerializeField] TextMeshProUGUI _upgradeCost;
    [SerializeField] UpgradeTickUI _upgradeTickPrefab;
    [SerializeField] Transform _upgradeTicksContainer;
    [SerializeField] Button _buyButton;

    public System.Action OnPurchased;

    int _currentLevel = 0;

    void Start()
    {
        if (_upgradeResource) LoadResource(_upgradeResource);
        
    }

    void LoadResource(UpgradeResource upgradeResource)
    {
        _upgradeResource = upgradeResource;
        _upgradeName.text = _upgradeResource.ShopName;
        UpdateState();
    }

    public int GetCost()
    {
        return (int) Mathf.Pow(_upgradeResource.StartingCost, _currentLevel + 1);
    }

    public void UpdateState()
    {
        _buyButton.interactable = PlayerBoat.Instance.Coins >= GetCost();

        _upgradeCost.text = GetCost().ToString();
        _upgradeLevel.text = $"Level {_currentLevel + 1}";

        foreach (Transform t in _upgradeTicksContainer) Destroy(t.gameObject);
        for(int i = 0; i < _upgradeResource.MaxLevel; i++)
        {
            UpgradeTickUI tick = Instantiate(_upgradeTickPrefab, _upgradeTicksContainer);
            tick.SetActive(_currentLevel > i);
        }
    }

    public void Purchase()
    {
        PlayerBoat.Instance.LoseCoins(GetCost());
        PlayerBoat.Instance.AddUpgrade(_upgradeResource);
        _currentLevel += 1;
        OnPurchased?.Invoke();
    }
}
