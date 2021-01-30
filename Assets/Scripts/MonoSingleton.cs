using UnityEngine;

public class MonoSingleton<T>: MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    void Awake()
    {
        instance = GetComponent<T>();
    }
}
