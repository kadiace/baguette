using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void RestartNow()
    {
        Managers.Scene.LoadScene(Scene.MainStage);
    }
    public void RestartAfter3Seconds()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Managers.Scene.LoadScene(Scene.MainStage);
    }
}
