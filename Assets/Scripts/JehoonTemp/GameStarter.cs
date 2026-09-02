using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Tooltip("게임 스타트 패널")]
    [SerializeField] private GameObject introductionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartButtonClicked()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        introductionPanel.SetActive(false);
    }
}
