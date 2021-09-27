using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginNetworkSingleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T _s;

    public static T S
    {
        get
        {
            if(_s == null)
            {
                _s = (T)FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return _s;
                }

                if (_s == null)
                {
                    GameObject singleton = new GameObject();
                    _s = singleton.AddComponent<T>();
                    singleton.name = "[" + typeof(T).ToString() + "]";

                    DontDestroyOnLoad(singleton);

                }
            }
            return _s;
        }
    }
}
