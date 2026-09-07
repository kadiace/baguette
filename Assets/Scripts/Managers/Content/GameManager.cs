using UnityEngine;

public class GameManager
{
    private bool _paused;

    public bool Paused
    {
        get { return _paused; }
        set
        {
            _paused = value;
            if (value)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;

            }

        }
    }

    public void Init()
    {
        _paused = false;
    }
}
