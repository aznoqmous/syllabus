using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Loot : MonoBehaviour
{

    [SerializeField] List<GameObject> _models;

    void Start()
    {
        foreach (GameObject model in _models) model.SetActive(false);
        _models[Mathf.FloorToInt(_models.Count * Random.value)].SetActive(true);
    }

    void Update()
    {
        
    }
}
