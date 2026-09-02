using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public OnOffManager shopOnOFF;
    [Header("상점 관련 UI")]
    [Tooltip("상점 UI")]
    public GameObject storeUI;
    [Tooltip("상호작용 키 UI (키 힌트 UI)")]
    public GameObject keyhintUI;

    [SerializeField] Transform player;

    void Awake()
    {
        keyhintUI.SetActive(false);
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        //플레이어 방향에 따라 키 힌트 회전
        Vector3 direction = player.position - keyhintUI.transform.position;
        direction.y = 0f;   //기울임 방지

        if (direction.sqrMagnitude > 0.001f)
        {
            //텍스트 앞면이 플레이어를 향하도록 회전값 계산 및 회전
            keyhintUI.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }

    //키 힌트 보여주기
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        player = other.transform;
        keyhintUI.SetActive(true);
        other.GetComponent<PlayerController>().SetShopInteration(this);
    }

    //키 힌트 숨기기
    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        player = null;
        keyhintUI.SetActive(false);
        other.GetComponent<PlayerController>().RemoveShopInteration();
    }

    public void ShowStore()
    {
        shopOnOFF.StateChange();
        storeUI.SetActive(true);
    }

}
