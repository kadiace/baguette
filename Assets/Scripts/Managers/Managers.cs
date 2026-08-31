using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private readonly DeliveryManager _deliveryManager = new();

    private static Managers Instance
    {
        get
        {
            EnsureExists();
            return _instance;
        }
    }

    public static DeliveryManager Delivery => Instance._deliveryManager;

    public static void EnsureExists()
    {
        if (_instance != null)
            return;

        Managers existing = FindAnyObjectByType<Managers>();
        if (existing != null)
        {
            _instance = existing;
            return;
        }

        GameObject go = GameObject.Find("@App");
        if (go == null)
            go = new GameObject("@App");

        Managers managers = go.GetComponent<Managers>();
        if (managers == null)
            managers = go.AddComponent<Managers>();

        _instance = managers;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        _deliveryManager.Init();
    }
}
