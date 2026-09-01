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
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
                root.AddComponent<Managers>();
            }
            return root;
        }
    }

    public T CreateUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject go = Managers.Resource.Instantiate($"UIs/SubItems/{name}");

        if (parent != null)
            go.transform.SetParent(parent);
        else
            go.transform.SetParent(_root.transform);

        return go.GetOrAddComponent<T>();
    }
}
