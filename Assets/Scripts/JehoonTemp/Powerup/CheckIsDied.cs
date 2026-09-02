using UnityEngine;

public class CheckIsDied : MonoBehaviour
{
    [Tooltip("플레이어 컨트롤러")]
    [SerializeField] private PlayerController playerController;
    [Tooltip("게임오버 오브젝트")]
    [SerializeField] private GameObject gameOverScreen;
    [Tooltip("씬 재시작")]
    [SerializeField] private RestartScene restartScene;

    void Start()
    {
        playerController.PlayerDied.AddListener(LoadGameOver);
    }

    // Update is called once per frame
    private void LoadGameOver()
    {
        gameOverScreen.SetActive(true);
        restartScene.RestartAfter3Seconds();
    }
}
