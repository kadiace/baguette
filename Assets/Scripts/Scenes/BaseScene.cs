using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Scene SceneType { get; protected set; } = Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = FindAnyObjectByType(typeof(EventSystem));
        if (obj == null)
        {
            Managers.Resource.Instantiate("UIs/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();
}
