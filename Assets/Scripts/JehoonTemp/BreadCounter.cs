using UnityEngine;

public class BreadCounter : MonoBehaviour
{
    [Tooltip("플레이어 오브젝트")]
    [SerializeField] private GameObject player;

    [Tooltip("플레이어 오브젝트 이름")]
    [SerializeField] private string playerName = "PlayerTemp";

    [Tooltip("플레이어 WeaponHandler")]
    [SerializeField] private WeaponHandler JHTmpPlayerWeaponHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Tooltip("현재 빵 개수")]
    [SerializeField] private int currentBread;

    [Tooltip("최대 빵 개수")]
    [SerializeField] private int maxBread;

    [Tooltip("빵 개수 표시 TextMesh")]
    [SerializeField] private TMPro.TextMeshProUGUI breadCountText;
    void Start()
    {
        UpdateBreadCounter(currentBread);
        player = GameObject.Find(playerName);
        maxBread = JHTmpPlayerWeaponHandler.GetMaxBread();
        currentBread = JHTmpPlayerWeaponHandler.GetCurrentBread();
        Debug.Log("BreadCounter Start() - maxBread: " + maxBread + ", currentBread: " + currentBread);

        breadCountText = GameObject.Find("BreadCount").GetComponent<TMPro.TextMeshProUGUI>();

        JHTmpPlayerWeaponHandler.OnBreadCountChanged.AddListener(UpdateBreadCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 빵 개수 업데이트
    void UpdateBreadCounter(int currentBread)
    {
        Debug.Log($"전달 받은 빵 개수: {currentBread}, 최대 빵 개수: {maxBread}");
        breadCountText.text = "Bread: " + currentBread.ToString() + " / " + maxBread.ToString();
    }
}
