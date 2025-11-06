using System.Collections.Generic;
using UnityEngine;

public class BulletsUI : MonoBehaviour
{

    [SerializeField] HeartUI _bulletUIPrefab;
    [SerializeField] GameObject _bulletUIContainer;
    List<HeartUI> _bulletUIs = new List<HeartUI>();


    void Start()
    {
        UpdateMaxBullets();
        UpdatePlayerBullets();
    }

    public void UpdateMaxBullets()
    {
        foreach (Transform t in _bulletUIContainer.transform) Destroy(t.gameObject);
        _bulletUIs.Clear();
        for (int i = 0; i < PlayerBoat.Instance.MaxBullets; i++) _bulletUIs.Add(Instantiate(_bulletUIPrefab, _bulletUIContainer.transform));
    }
    public void UpdatePlayerBullets()
    {
        for (int i = 0; i < _bulletUIs.Count; i++)
        {
            HeartUI heart = _bulletUIs[i];
            //heart.SetState(i < PlayerBoat.Instance.CurrentBullets);
            heart.Image.color = i < PlayerBoat.Instance.CurrentBullets ? Color.white : Color.clear;
        }
    }

}
