using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetorAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetorAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return Util.IsValid(go);
    }
}
