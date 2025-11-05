using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Loot : MonoBehaviour
{

    [SerializeField] List<LootResource> _lootResources;
    [SerializeField] Transform _modelContainer;
    LootResource _resource;
    public LootResource Resource { get { return _resource;  } }

    private void Start()
    {
        LoadResource(_lootResources.PickRandom());
    }

    void LoadResource(LootResource resource)
    {
        foreach (Transform t in _modelContainer) Destroy(t.gameObject);
        Instantiate(resource.Model, _modelContainer);
        _resource = resource;
    }
}
