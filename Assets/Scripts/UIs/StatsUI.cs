using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] HeartUI _heartUIPrefab;
    [SerializeField] GameObject _heartsContainer;

    List<HeartUI> _hearts = new List<HeartUI>();

    public static StatsUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdatePlayerMaxHp();
        UpdatePlayerHP();
    }

    void Update()
    {
        
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
}
