using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (GameObject.Find(typeof(T).Name) == null)
            {
                GameObject go = new GameObject() { name = typeof(T).Name };
                DontDestroyOnLoad(go);
                if (go.GetComponent<T>() == null)
                {
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
