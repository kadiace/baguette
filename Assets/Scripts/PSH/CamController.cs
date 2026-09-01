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
    }
    
    void LateUpdate()
    {
        if (!isOver){
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
                transform.position = Vector3.Lerp(transform.position, scopeTransform.position, Time.deltaTime * adsSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, Time.deltaTime * adsSpeed);
                //1인칭일 때만 무기 상하 회전
                weapon.rotation = Quaternion.Slerp(transform.rotation, camRotation, Time.deltaTime * adsSpeed);
            }
            else
            {
                //카메라 위치 조절 및 시선 고정
                transform.position = player.transform.position + (camRotation * camOffset);
                transform.LookAt(player.transform.position + Vector3.up * 1f);
                //무기 상 하 방향 초기화
                weapon.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    
}
