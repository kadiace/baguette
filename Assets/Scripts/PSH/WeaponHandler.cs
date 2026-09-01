using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private CamController camController;
    [SerializeField] private Animator weaponHandlerAni;

    [Header("무기 설정")]
    [Tooltip("무기 프리팹(바게트 빵)")]
    public Baguette BreadPrefs;
    public Baguette onHandBread;
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
    [SerializeField] private float throwForce;
    [Tooltip("빵 재장전 시간")]
    [SerializeField] private float reloadTime;

    [Header("위치 확인용 빵(테스트 후 제거 예정)")]
    public GameObject breadTMP;

       // UI에 전달할 이벤트
    [Tooltip("빵 개수 변경 이벤트")]
    public UnityEvent<int> OnBreadCountChanged = new UnityEvent<int>();

    //시작 시점에 빵 갯수 초기화  
    void Start()
    {
        breadTMP.SetActive(false);
        curBread = 99999; //테스트용: curBread = MaxBread;
        CreateBread(0);
        //UI 표시 빵 갯수 초기화
        CountEventInvoke();
    }

#region 빵 사용 관련

    /// <summary>
    /// 빵 충전/보급
    /// </summary>
    public void SupplyBread()
    {
        curBread = MaxBread;
        CountEventInvoke();
    }

    /// <summary>
    /// 빵 최대 보유 갯수 증가
    /// </summary>
    /// <param name="amount">증가량</param>
    public void UpgradeMaxBread(int amount)
    {
        MaxBread += amount;
        curBread = MaxBread;
        CountEventInvoke();
    }

    public void CreateBread(int type)
    {
        if(curBread <= 0)
        {
            Debug.Log("빵이 없습니다.");
            return;
        }

        if(type == 0)
            curBread--;

        CountEventInvoke();

        //빵 프리팹 생성
        onHandBread = Instantiate(BreadPrefs, transform);
        //빵 위치 조정
        onHandBread.transform.localPosition = new Vector3(0.62f, 0.2f, 0.7f);
        onHandBread.transform.localRotation = Quaternion.identity;
        onHandBread.transform.localScale = Vector3.one;
    }

#endregion

#region 빵 공격 관련
    /// <summary>
    /// 근접 공격
    /// </summary>
    public void MeleeAttack()
    {
        //애니메이션과 콜라이더를 통해 처리
        Debug.Log("빵 휘두르기! \n          효과가 별로인 듯하다...");
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
            Debug.Log("빵 던지기! \n        효과가 굉장했다!");
            //curBread--;
            isCooldown = true;
            CountEventInvoke();
            //빵 던지기
            onHandBread.ThrowBaguette(throwForce);
            //빵 재장전
            weaponHandlerAni.Play("ReloadBaguette");
            //시간 측정
            StartCoroutine(ThrowCooldown());
            StartCoroutine(ThrowBreadCoroutine());
        }
    }
#endregion

#region 빵 개수 관련 by.Jeehoon
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
        //Debug.Log("현재 빵 개수: " + curBread);
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
        //재장전 시간 동안 조준 상태 유지
        yield return new WaitForSeconds(reloadTime);

        //카메라 줌 아웃(3인칭으로 변경)
        camController.CameraAim(false);
        yield return null;
    }
}
