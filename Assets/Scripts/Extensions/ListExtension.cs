using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class ListExtension
{
    public static T PickRandom<T>(this List<T> list)
    {
        return list.Count > 0 ? list[Random.Range(0, list.Count)] : default;
    }

    public static List<T> PickRandom<T>(this List<T> list, int count=1)
    {
        List<T> resList = new List<T>(list);
        resList.Shuffle();
        return resList.Count > 0 ? resList.GetRange(0, count) : default;
    }

    public static List<T> GetNearests<T> (this List<T> list, Vector3 position) where T : MonoBehaviour
    {
        list = list.Clone();
        list.SortByNearest(position);
        return list;
    }

    public static List<T> Clone<T> (this List<T> list)
    {
        return new List<T>(list);
    }

    public static T GetNearest<T>(this List<T> list, Vector3 position) where T : MonoBehaviour
    {
        list = list.GetNearests(position);
        return list.Count > 0 ? list.First() : null;
    }

    public static void SortByNearest<T>(this List<T> list, Vector3 position) where T : MonoBehaviour
    {
        list.Sort((T a, T b) => (int) Mathf.Sign(a.transform.position.DistanceTo(position) - b.transform.position.DistanceTo(position)));
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}