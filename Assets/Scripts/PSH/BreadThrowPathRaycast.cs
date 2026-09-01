using UnityEngine;

public class BreadThrowPathRaycast : MonoBehaviour
{
    [Header("레이케스트 설정")]
   [SerializeField] private float raycastDistance;
   bool isAming = false;
   [Tooltip("시각 효과")]
   private LayerMask hitLayer = ~0; 
   [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineWidth;
   [SerializeField] private GameObject hitDotMarker; // 충돌 지점에 띄울 점/마커 오브젝트
   [SerializeField] private float markerScale = 0.05f;

    /// <summary>
    /// 라인 렌더러 준비
    /// </summary>
    void Awake()
    {
        // 월드 좌표계 사용 (충돌 지점을 직접 찍어야 하므로 true)
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.alignment = LineAlignment.View; // 카메라 정면을 바라보게 해 선 두께 유지
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        HideThrowPath();
    }

    void Update()
    {
        if(isAming)
        {
            UpdateRaycast();
        }
    }

    void UpdateRaycast()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        Vector3 targetPoint;

        //레이캐스트 발사
        if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance, hitLayer, triggerInteraction))
        {
            targetPoint = hit.point;

            //충돌 지점에 빨간 점(마커) 배치
            if (hitDotMarker != null)
            {
                hitDotMarker.SetActive(true);
                hitDotMarker.transform.position = hit.point + (hit.normal * 0.01f);
                hitDotMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                //마커 크기 조절: 카메라와의 거리에 따라 마커 크기 조절 (플레아어에게는 거리에 상관 없이 항상 같은 크기로 보임)
                Camera cam = Camera.main;
                if (cam != null)
                {
                    //카메라와의 실제 거리 계산
                    float distanceToCamera = Mathf.Sqrt(Vector3.Distance(cam.transform.position, hitDotMarker.transform.position * 5f));
                
                    //거리에 비례한 스케일 적용
                    hitDotMarker.transform.localScale = Vector3.one * (distanceToCamera * markerScale);
                }
            }
        }
        else
        {
            //허공에 쐈을 때: 최대 사거리까지 선 연결 + 점 숨김
            targetPoint = origin + (direction * raycastDistance);

            if (hitDotMarker != null)
            {
                hitDotMarker.SetActive(false);
            }
        }

        //LineRenderer로 시작점과 끝점 연결해서 그리기
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, targetPoint);
    }

    public void DrowThrowPath()
    {
        isAming = true;
        lineRenderer.enabled = true;
    }

    public void HideThrowPath()
    {
        isAming = false;
        lineRenderer.enabled = false;
        if (hitDotMarker != null)
        {
            hitDotMarker.SetActive(false);
        }
    }
}