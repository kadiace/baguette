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

    public static void BindEvent(
    this GameObject go,
    Action<PointerEventData> action,
    UIEvent type = UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static void ClearEvent(this GameObject go)
    {
        UI_Base.ClearEvent(go);
    }
}
