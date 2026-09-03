using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    // 정적 싱글톤 인스턴스
    public static MinimapManager Instance { get; private set; }

    [Header("미니맵 요소")]
    [SerializeField] private GameObject minimapUI;          // 미니맵 전체 루트 오브젝트 (켜고 끄기용)
    [SerializeField] private RectTransform minimapRect;     // 렌더 텍스처를 보여주는 RawImage의 RectTransform
    [SerializeField] private RectTransform playerMark;
    [SerializeField] private RectTransform playerSight;

    [Header("플레이어 및 카메라")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Camera mainCamera;
    private void Awake()
    {
        // 싱글톤 중복 방지 및 인스턴스 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void LateUpdate()
    {
        UpdatePlayerIcon();
    }

    private void UpdatePlayerIcon()
    {
        //월드 좌표 -> 뷰포트 좌표 변환
        Vector3 viewPos = minimapCamera.WorldToViewportPoint(playerTransform.position);

        //미니맵 Rect 사이즈 기준으로 위치 계산 (Anchor/Pivot이 0.5, 0.5 기준)
        Vector2 size = minimapRect.rect.size;
        Vector2 localPos = new Vector2(
            (viewPos.x - 0.5f) * size.x,
            (viewPos.y - 0.5f) * size.y
        );

        playerMark.anchoredPosition = localPos;

        // Apply main camera rotation
        float yRotation = mainCamera.transform.eulerAngles.y;
        float rotation = -yRotation + 45;
        Vector3 targetEuler = playerSight.transform.eulerAngles;
        targetEuler.z = rotation;

        playerSight.transform.eulerAngles = targetEuler;

        float width = playerSight.rect.width;
        playerSight.anchoredPosition = localPos +
            new Vector2(Mathf.Cos((90 - yRotation) * Mathf.Deg2Rad), Mathf.Sin((90 - yRotation) * Mathf.Deg2Rad))
            * width / Mathf.Sqrt(2);
    }

    /// <summary>
    /// 미니맵 UI 켜기
    /// </summary>
    public void HideMinimap()
    {
        if (minimapUI != null)
            minimapUI.gameObject.SetActive(false);

    }

    /// <summary>
    /// 미니맵 UI 끄기
    /// </summary>
    public void ShowMinimap()
    {
        if (minimapUI != null)
            minimapUI.gameObject.SetActive(true);
    }
}