using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class JHTmpWeaponHandler : MonoBehaviour
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

    // UI에 전달할 이벤트
    [Tooltip("빵 개수 변경 이벤트")]
    public UnityEvent<int> OnBreadCountChanged;


    //시작 시점에 빵 갯수 초기화
    void Start()
    {
        curBread = MaxBread;
        OnBreadCountChanged = new UnityEvent<int>();
        CountEventInvoke();
    }

#region 빵 사용 관련
    /// <summary>
    /// 빵 사용 시 현재 빵 개수를 감소시키고, UI에 변경 사항을 전달합니다.
    /// </summary>
    void UseBread()
    {
        if (curBread > 0)
        {
            curBread--;
            CountEventInvoke();
            Instantiate(BreadPrefs, transform.position + transform.forward * 1.5f, transform.rotation);
        }
    }
    /// <summary>
    /// 현재 빵 개수를 최대치로 재장전하고, UI에 변경 사항을 전달합니다.
    /// </summary>
    public void ReloadBread()
    {
        curBread = MaxBread;
        CountEventInvoke();
    }
    /// <summary>
    /// 최대 빵 개수를 증가시키고, 현재 빵 개수를 최대치로 설정하며, UI에 변경 사항을 전달합니다.
    /// </summary>
    /// <param name="amount">최대 빵 개수를 증가시킬 양을 매개변수로 전달받습니다.</param>
    public void UpgradeMaxBread(int amount)
    {
        MaxBread += amount;
        curBread = MaxBread;
        CountEventInvoke();
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
        else if (curBread <= 0){
            Debug.Log("빵이 없습니다.");
            return;
        }
        else
        {
            Debug.Log("빵 던지기!");
            curBread--;
            isCooldown = true;
            CountEventInvoke();
            //빵 던지기
            BreadPrefs.ThrowBaguette(throwForce);
            //빵 재장전
            // weaponHandlerAni.Play("ReloadBaguette");
            //시간 측정
            StartCoroutine(ThrowCooldown());
            StartCoroutine(ThrowBreadCoroutine());
        }
    }

#endregion

#region 빵 개수 관련
    /// <summary>
    /// 현재 빵 개수를 반환합니다.
    /// </summary>
    /// <returns>현재 빵 개수</returns>
    public int GetCurrentBread()
    {
        return curBread;
    }
    /// <summary>
    /// 최대 빵 개수를 반환합니다.
    /// </summary>
    /// <returns>최대 빵 개수</returns>
    public int GetMaxBread()
    {
        return MaxBread;
    }
    /// <summary>
    /// 현재 빵 개수가 변경되었음을 알리는 이벤트를 Invoke합니다.
    /// </summary>
    private void CountEventInvoke()
    {
        OnBreadCountChanged.Invoke(curBread);
        Debug.Log("현재 빵 개수: " + curBread);
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
        // camController.CameraAim(false);
    }
}
