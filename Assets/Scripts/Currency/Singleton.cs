using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    public string scriptName = "";

    private static T _instance = null;
    private static readonly object threadSafeLock = new object();

    public static T Instance
    {
        get
        {
            if(_instance==null)
            {
                lock(threadSafeLock)
                {
                    if(_instance)
                    {
                        GameObject go = new GameObject();
                        _instance = go.AddComponent<T>();
                        go.name = $"Singleton_{(_instance as Singleton<T>).scriptName}";
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return _instance;
        }
    }
}
