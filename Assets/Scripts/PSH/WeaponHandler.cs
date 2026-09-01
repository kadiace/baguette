using UnityEngine;
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


    //시작 시점에 빵 갯수 초기화  
    void Start()
    {
        breadTMP.SetActive(false);
        curBread = 99999;
        CreateBread(0);
    }

#region 빵 사용 관련

    /// <summary>
    /// 빵 충전/보급
    /// </summary>
    public void SupplyBread() => curBread = MaxBread;

    /// <summary>
    /// 빵 최대 보유 갯수 증가
    /// </summary>
    /// <param name="amount">증가량</param>
    public void UpgradeMaxBread(int amount)
    {
        MaxBread += amount;
        curBread = MaxBread;
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
        else
        {
            Debug.Log("빵 던지기! \n        효과가 굉장했다!");
            //curBread--;
            isCooldown = true;
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
