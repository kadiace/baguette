using UnityEngine;

public class HealthCounter : MonoBehaviour
{

    [Tooltip("플레이어 오브젝트 이름")]
    [SerializeField] private string playerName;

    [Tooltip("현재 체력 칸 prefab")]
    [SerializeField] private GameObject HealthPrefab;

    [Tooltip("잃은 체력 칸 prefab")]
    [SerializeField] private GameObject LostHealthPrefab;

    void Start()
    {
        Managers.Player.HealthCounter = this;
    }

    /// <summary>
    /// 플레이어 체력 변동 이벤트를 받아서 체력 UI 업데이트
    /// </summary>
    /// <param name="currentHealth">현재 체력</param>
    public void UpdateHealthCounter()
    {
        int maxPlayerHealth = Managers.Player.PlayerStat.MaxHp;

        int playerHealth = Managers.Player.PlayerStat.Hp;

        // 기존 체력 UI 제거
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 체력 UI 생성
        for (int i = 0; i < maxPlayerHealth; i++)
        {
            if (i < playerHealth)
                Instantiate(HealthPrefab, transform);
            else
                Instantiate(LostHealthPrefab, transform);
        }
    }
}
