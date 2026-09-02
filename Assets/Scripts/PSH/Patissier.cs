using UnityEngine;

public class Patissier : MonoBehaviour
{
    [Header("빵집 관련 UI")]
    [Tooltip("상호작용 키 UI (키 힌트 UI)")]
    public GameObject keyhintUI;

    [Tooltip("키 힌트가 바라볼 대상(카메라)")]
    [SerializeField] Transform target;

    void Awake()
    {
        keyhintUI.SetActive(false);
    }

    /// <summary>
    /// 키 힌트 표시는 "카메라"를 바라봄 
    /// </summary>
    void LateUpdate()
    {
        if (target == null)
            return;
        keyhintUI.transform.rotation = target.rotation;
    }

    //키 힌트 보여주기
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        target = Camera.main.transform;
        keyhintUI.SetActive(true);
        other.GetComponent<PlayerController>().SetBreadShopInteration(this);
    }

    //키 힌트 숨기기
    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        target = null;
        keyhintUI.SetActive(false);
        other.GetComponent<PlayerController>().RemoveBreadShopInteration();
    }
}
