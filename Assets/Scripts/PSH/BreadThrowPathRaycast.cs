using UnityEngine;

public class BreadThrowPathRaycast : MonoBehaviour
{
   [SerializeField] private float raycastDistance = 1000f;
   [SerializeField] private LineRenderer lineRenderer;

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // 오브젝트의 이동 및 회전을 자동으로 따라가도록 로컬 좌표계 사용
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;

        // 원점(0,0,0)에서 정면(Z축 앞방향)으로 지정한 거리만큼 선 설정
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.forward * raycastDistance);

        HideThrowPath();
    }

    public void DrowThrowPath()
    {
        lineRenderer.enabled = true;
    }

    public void HideThrowPath()
    {
        lineRenderer.enabled = false;
    }
}
