using UnityEngine;

public class GameComplete : MonoBehaviour
{
    [SerializeField] private GameObject completePanel;

    public void CompleteGame()
    {
        // 게임 정지
        Time.timeScale = 0f;

        // 엔딩 패널 활성화
        completePanel.SetActive(true);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
