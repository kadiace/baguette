using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("플레이어 오브젝트")]
    [SerializeField] private GameObject player;

    [Tooltip("플레이어 체력")]
    [SerializeField] private int playerHealth;

    [Tooltip("플레이어 최대 체력")]
    [SerializeField] private int maxPlayerHealth;

    [Tooltip("플레이어 오브젝트 이름")]
    [SerializeField] private string playerName = "PlayerTemp";

    [Tooltip("현재 체력 칸 prefab")]
    [SerializeField] private GameObject HealthPrefab;

    [Tooltip("잃은 체력 칸 prefab")]
    [SerializeField] private GameObject LostHealthPrefab;

    void Start()
    {
        player = GameObject.Find(playerName);
        maxPlayerHealth = 5;
        playerHealth = maxPlayerHealth;
        UpdateHealthCounter(playerHealth);

        player.GetComponent<JHTmpPlayerController>().OnHealthChanged.AddListener(UpdateHealthCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 플레이어 체력 변동 이벤트를 받아 체력 UI를 업데이트합니다.
    /// </summary>
    /// <param name="currentHealth">현재 체력</param>
    public void UpdateHealthCounter(int currentHealth)
    {
        playerHealth = currentHealth;

        // 기존 체력 UI 제거
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 체력 UI 생성
        for (int i = 0; i < maxPlayerHealth; i++)
        {
            if (i < playerHealth)
            {
                Instantiate(HealthPrefab, transform);
            }
            else
            {
                Instantiate(LostHealthPrefab, transform);
            }
        }
    }
}
