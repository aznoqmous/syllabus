using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] HeartUI _heartUIPrefab;
    [SerializeField] GameObject _heartsContainer;
    [SerializeField] Transform _renderedCoinTransform;
    [SerializeField] TextMeshProUGUI _coinText;

    List<HeartUI> _hearts = new List<HeartUI>();

    public static StatsUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateCoins();
        UpdatePlayerMaxHp();
        UpdatePlayerHP();
    }

    void Update()
    {
        _renderedCoinTransform.eulerAngles -= new Vector3(0, Time.deltaTime * 20f, 0);
        _renderedCoinTransform.localScale = Vector3.Lerp(_renderedCoinTransform.localScale, Vector3.one * 3f, Time.deltaTime * 10f);
    }

    public void UpdatePlayerMaxHp()
    {
        foreach (Transform t in _heartsContainer.transform) Destroy(t.gameObject);
        _hearts.Clear();
        for (int i = 0; i < PlayerBoat.Instance.MaxHp; i++) _hearts.Add(Instantiate(_heartUIPrefab, _heartsContainer.transform));
    }

    public void UpdatePlayerHP()
    {
        for(int i = 0; i < _hearts.Count; i++)
        {
            HeartUI heart = _hearts[i];
            heart.SetState(i < PlayerBoat.Instance.CurrentHp);
        }
    }

    public void UpdateCoins()
    {
        _renderedCoinTransform.localScale *= 2f;
        _coinText.text = PlayerBoat.Instance.Coins.ToString();
    }
}
