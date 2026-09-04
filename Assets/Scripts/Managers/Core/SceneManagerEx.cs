using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene
    {
        get { return Object.FindAnyObjectByType<BaseScene>(); }
    }
    public void LoadScene(Scene name)
    {
        Managers.Clear();
        SceneManager.LoadScene(Util.GetEnumName(name));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
