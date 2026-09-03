using UnityEngine;
using UnityEngine.InputSystem; //마우스 입력을 받기 위해 필요

public class CamController : MonoBehaviour
{
    Vector3 camOffset = new Vector3(0, 4, -4.5f);
    public Transform scopeTransform;
    [Header("카메라 설정")]
    public float camRotateSpeed;
    public float adsSpeed = 15f;
    float maxPitch = 45f;
    float minPitch = -50f;
    [SerializeField] float mouseX;   //마우스 좌우
    [SerializeField] float mouseY;   //마우스 상하
    bool isOver = false;
    bool isZoom = false;
    private bool isAimingDone = false;

    [Header("카메라 회전에 따라 같이 회전 시킬 오브젝트")]
    [Tooltip("좌 우 회전")]
    public GameObject player;
    [Tooltip("상 하 회전 (1인칭 시)")]
    public Transform weapon;

    /// <summary>
    /// 컨포넌트 할당 시 자동으로 변수 값 할당
    /// </summary>
    void Reset()
    {
        camRotateSpeed = 30f;
        //외부 게임오브젝트들 설정
        player = GameObject.Find("PlayerTemp");
        //weapon = player.transform.Find("WeaponHandler").transform;
        //scopeTransform = player.transform.Find("FirstPersonCamPos").transform;
    }

    public void FreezeCam()
    {
        isOver = true;
    }

    public void CameraAim(bool isAim)
    {
        isZoom = isAim;
        if (isAim)
            MinimapManager.Instance.HideMinimap();
        else
            MinimapManager.Instance.ShowMinimap();
    }

    void LateUpdate()
    {
        if (!isOver)
        {
            //마우스 움직임에 따른 카메라 방향 계산 (회전)
            mouseX += Mouse.current.delta.x.ReadValue() * camRotateSpeed * Time.deltaTime;
            mouseY -= Mouse.current.delta.y.ReadValue() * camRotateSpeed * Time.deltaTime;
            mouseY = Mathf.Clamp(mouseY, minPitch, maxPitch);
            Quaternion camRotation = Quaternion.Euler(mouseY, mouseX, 0);

            //플레이어 회전
            player.transform.rotation = Quaternion.Euler(0, mouseX, 0);

            //카메라 위치 조정
            if (isZoom)
            {
                if (isAimingDone)
                {
                    //플레이어가 뛰든 점프하든 위치 고정 (밀림 현상 해결)
                    transform.position = scopeTransform.position;
                    transform.rotation = camRotation;
                    weapon.rotation = camRotation;
                }
                else
                {
                    //전환 연출 구간
                    transform.position = Vector3.Lerp(transform.position, scopeTransform.position, Time.deltaTime * adsSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, Time.deltaTime * adsSpeed);
                    weapon.rotation = Quaternion.Slerp(weapon.rotation, camRotation, Time.deltaTime * adsSpeed);

                    // 목표 지점에 근접하면 즉시 조준 완료 상태로 락(Lock)
                    if (Vector3.Distance(transform.position, scopeTransform.position) < 0.15f)
                    {
                        isAimingDone = true;
                        transform.position = scopeTransform.position;
                        transform.rotation = camRotation;
                        weapon.rotation = camRotation;
                    }
                }
            }
            else
            {
                isAimingDone = false;
                //카메라 위치 조절 및 시선 고정
                transform.position = player.transform.position + (camRotation * camOffset);
                transform.LookAt(player.transform.position + Vector3.up * 1f);
                //무기 상 하 방향 초기화
                weapon.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

}
