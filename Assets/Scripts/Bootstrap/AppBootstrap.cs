using UnityEngine;

public static class AppBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        Managers.EnsureExists();
    }
}