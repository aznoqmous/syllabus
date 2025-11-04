using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] Foe _foeShipPrefab;
    [SerializeField] float _spawnDistance = 20f;

    void Start()
    {
        SpawnFoe();   
    }

    void Update()
    {
        
    }

    void SpawnFoe() {
        Vector3 position = PlayerBoat.Instance.transform.position + Quaternion.Euler(0, Random.value * 360f, 0) * Vector3.left * _spawnDistance;
        Foe newFoe = Instantiate(_foeShipPrefab, position, Quaternion.identity);

    }
}
