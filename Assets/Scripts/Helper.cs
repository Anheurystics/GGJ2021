using UnityEngine;

public static class Helper
{
    public static T PickRandom<T>(this T[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}
