using UnityEngine;

public class Util
{
    public static T GetorAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        return component ? component : go.AddComponent<T>();
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        return FindChild<Transform>(go, name, recursive)?.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;
        if (recursive)
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }

        return null;
    }

    public static string RemoveUnityCloneSuffix(string name)
    {
        const string suffix = "(Clone)";
        return name.EndsWith(suffix)
            ? name[..^suffix.Length]
            : name;
    }

    public static string GetEnumName<T>(T element)
    {
        return System.Enum.GetName(typeof(T), element);
    }

    public static bool IsValid(GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
