using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] List<Foe> _foeShipPrefabs;
    [SerializeField] float _spawnDistance = 100f;

    [SerializeField] float _credits = 0f;
    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] int _maxFoes = 8;
    float _nextSpawnTime = 0f;

    List<Foe> _foeList = new List<Foe>();

    void Update()
    {
        _credits += Time.deltaTime * (Time.time / 60f + 1f);
        if (Time.time > _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + _spawnInterval;
            Spawn();
        }
    }

    void Spawn()
    {
        print(_credits);
        bool spawned = false;
        while (true)
        {
            if (_foeList.Count >= _maxFoes) break;
            List<Foe> foes = GetAvailableFoes();
            if (foes.Count <= 0) break;
            Foe newFoe = foes.PickRandom();
            _credits -= newFoe.Cost;
            SpawnFoe(newFoe);
            spawned = true;
        }
        if (!spawned)
        {
            _nextSpawnTime += Random.value * _spawnInterval;
        }

    }

    List<Foe> GetAvailableFoes()
    {
        return _foeShipPrefabs.Where((Foe foe) => foe.Cost < _credits).ToList();
    }

    Foe SpawnFoe(Foe foe) {
        Vector3 position = PlayerBoat.Instance.transform.position + Quaternion.Euler(0, Random.value * 360f, 0) * Vector3.left * _spawnDistance;
        Foe newFoe = Instantiate(foe, position, Quaternion.identity);
        _foeList.Add(newFoe);
        newFoe.Ship.OnDie += () => _foeList.Remove(newFoe);
        return newFoe;
    }
}
