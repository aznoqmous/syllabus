using System.Collections.Generic;
using UnityEngine;

public class FoliageManager : MonoBehaviour
{
    [SerializeField] float _maxDistance = 100f;
    [SerializeField] float _hideDistance = 90f;
    [SerializeField] float _randomRadius = 10f;
    [SerializeField] float _foliageSize = 1.5f;

    void Update()
    {
        if (!PlayerBoat.Instance) return;
        foreach(Transform t in transform)
        {
            float distance = t.position.DistanceTo(PlayerBoat.Instance.transform.position);
            if (distance > _maxDistance)
            {
                t.position = PlayerBoat.Instance.GetRandomProjectedPosition(45f, _hideDistance - _randomRadius, _hideDistance + _randomRadius);
                t.localScale = Vector3.zero;
                t.eulerAngles = Quaternion.AngleAxis(Random.value * 360f, Vector3.up) * t.eulerAngles;
            }
            t.localScale = Vector3.Lerp(t.localScale, distance > _hideDistance ? Vector3.zero : Vector3.one * _foliageSize, Time.deltaTime * 5f);
        }
    }
}
