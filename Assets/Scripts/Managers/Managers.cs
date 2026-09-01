using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private static Managers Instance
    {
        get
        {
            EnsureExists();
            return _instance;
        }
    }

    private readonly DeliverManager _deliverManager = new();
    private readonly PoolManager _poolManager = new();
    private readonly ResourceManager _resourceManager = new();
    private readonly SceneManagerEx _sceneManager = new();
    private readonly UIManager _uiManager = new();


    public static DeliverManager Deliver => Instance._deliverManager;
    public static PoolManager Pool => Instance._poolManager;
    public static ResourceManager Resource => Instance._resourceManager;
    public static SceneManagerEx Scene => Instance._sceneManager;
    public static UIManager UI => Instance._uiManager;

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
        _deliverManager.Init();
        _poolManager.Init();
    }

    public static void Clear()
    {
    }
}
