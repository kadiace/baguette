using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private CamController camController;
    [SerializeField] private Animator weaponHandlerAni;
    [Header("무기 설정")]
    [Tooltip("무기 프리팹(바게트 빵)")]
    public Baguette BreadPrefs;
    [Tooltip("최대 빵 보유 갯수")]
    [SerializeField] private int MaxBread = 5;
    [Tooltip("현재 빵 보유 횟수, 자동으로 초기화")]
    [SerializeField] private int curBread;
    [Header("원거리 공격 정보")]
    [Tooltip("쿨타임")]
    [SerializeField] private float throwCooldownTime;
    [Tooltip("쿨타임 여부")]
    [SerializeField] private bool isCooldown = false;
    [Tooltip("빵 던지는 힘(속도)")]
    [SerializeField] private float throwForce = 1f;
    [Tooltip("빵 재장전 시간")]
    [SerializeField] private float reloadTime = 3f;


    //시작 시점에 빵 갯수 초기화  
    void Start()
    {
        curBread = MaxBread;
    }

#region 빵 사용 관련
    void UseBread()
    {
        if (curBread > 0)
        {
            curBread--;
            Instantiate(BreadPrefs, transform.position + transform.forward * 1.5f, transform.rotation);
        }
    }

    public void ReloadBread()
    {
        curBread = MaxBread;
    }

    public void UpgradeMaxBread(int amount)
    {
        MaxBread += amount;
        curBread = MaxBread;
    }
#endregion

#region 빵 공격 관련
    /// <summary>
    /// 근접 공격
    /// </summary>
    public void MeleeAttack()
    {
        Debug.Log("빵 휘두르기! (플레이어가 애니메이션 진행");
    }

    /// <summary>
    /// 빵 던지기
    /// </summary>
    public void ThrowBread()
    {
        if (isCooldown){
            Debug.Log("아직 쿨타임이 남았습니다.");
            return;
        }
        else
        {
            Debug.Log("빵 던지기!");
            //curBread--;
            isCooldown = true;
            //빵 던지기
            BreadPrefs.ThrowBaguette(throwForce);
            //빵 재장전
            weaponHandlerAni.Play("ReloadBaguette");
            //시간 측정
            StartCoroutine(ThrowCooldown());
            StartCoroutine(ThrowBreadCoroutine());
        }
    }

#endregion
    /// <summary>
    /// 던지기 쿨타임 코루틴
    /// </summary>
    IEnumerator ThrowCooldown()
    {

        float curCoolTime = 0;
        while (curCoolTime < throwCooldownTime){
            curCoolTime++;
            yield return new WaitForSeconds(1f);
        }
        isCooldown = false;
    }

    /// <summary>
    /// 조준 상태 유지 코루틴(재장전 시간 연동)
    /// </summary>
    IEnumerator ThrowBreadCoroutine()
    {
        float curReloadTime = 0;
        while (curReloadTime < reloadTime){
            curReloadTime++;
            yield return new WaitForSeconds(1f);
        }
        camController.CameraAim(false);
    }
}
