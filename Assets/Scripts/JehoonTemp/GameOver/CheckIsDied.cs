using System.Collections;
using UnityEngine;

public class CheckIsDied : MonoBehaviour
{
    [Tooltip("플레이어 컨트롤러")]
    [SerializeField] private PlayerController playerController;
    [Tooltip("상점매니저")]
    [SerializeField] private ShopManager shopManager;
    [Tooltip("게임오버 오브젝트")]
    [SerializeField] private GameObject gameOverScreen;
    [Tooltip("게임클리어 배경")]
    [SerializeField] private GameObject gameClearScreen;
    [Tooltip("게임클리어 텍스트")]
    [SerializeField] private GameObject gameClearText;
    [Tooltip("재시작 버튼")]
    [SerializeField] private GameObject restartButton;
    [Tooltip("씬 재시작")]
    [SerializeField] private RestartScene restartScene;

    void Start()
    {
        playerController.PlayerDied.AddListener(LoadGameOver);
        shopManager.onAirConditionerPurchased.AddListener(LoadGameClear);
    }

    // Update is called once per frame
    private void LoadGameOver()
    {
        gameOverScreen.SetActive(true);
        restartScene.RestartAfter3Seconds();
    }

    public void LoadGameClear()
    {
        StartCoroutine(GameClearStaging());
    }

    IEnumerator GameClearStaging()
    {
        gameClearScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);

        gameClearText.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);

        restartButton.SetActive(true);
    }
}
