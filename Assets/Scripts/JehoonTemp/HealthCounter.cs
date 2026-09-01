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
        playerHealth = maxPlayerHealth - 3;
        UpdateHealthCounter(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthCounter(int currentHealth)
    {
        playerHealth = currentHealth;
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
