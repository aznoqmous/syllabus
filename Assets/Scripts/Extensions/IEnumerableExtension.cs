using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IEnumerableExtension
{
    public static IEnumerable<T> Map<T>(this IEnumerable<T> array, Action<T> callback)
    {
        List<T> copy = new List<T>(array);
        foreach(T item in copy)
        {
            callback(item);
        }
        return array;
    }
    public static bool Some<T>(this IEnumerable<T> array, Func<T, bool> callback)
    {
        List<T> copy = new List<T>(array);
        foreach (T item in copy)
        {
            if(callback(item)) return true;
        }
        return false;
    }

}
