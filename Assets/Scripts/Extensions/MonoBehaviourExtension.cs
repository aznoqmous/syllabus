using Unity.VisualScripting;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static float DistanceTo(this MonoBehaviour mb, MonoBehaviour to) {
        return mb.transform.position.DistanceTo(to.transform.position);
    }

    public static float DistanceTo(this MonoBehaviour mb, Transform to)
    {
        return mb.transform.position.DistanceTo(to.position);
    }

    public static float DistanceTo(this MonoBehaviour mb, Vector3 to)
    {
        return mb.transform.position.DistanceTo(to);
    }


    public static Vector3 DirectionTo(this MonoBehaviour mb, MonoBehaviour to)
    {
        return to.transform.position - mb.transform.position;
    }

    public static void Clear(this MonoBehaviour mb)
    {
        foreach (Transform t in mb.transform) Transform.Destroy(t.gameObject);
    }
}
