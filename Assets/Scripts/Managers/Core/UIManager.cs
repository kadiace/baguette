using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UIManager
{
    public GameObject _root
    {
        get
        {
            GameObject root = GameObject.Find("@App");
            return root;
        }
    }

    public T CreateUI<T>(Transform parent = null, string path = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject go = Managers.Resource.Instantiate($"UIs/{path}/{name}");

        if (parent != null)
            go.transform.SetParent(parent);
        else
            go.transform.SetParent(_root.transform);

        return go.GetOrAddComponent<T>();
    }
}
