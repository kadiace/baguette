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
    [Header("원거리 공격 정보")]
    [Tooltip("발사 각도 측정용")]
    [SerializeField] private Transform fireAngleTransform;
    [Tooltip("원거리 공격 경로(Raycast)")]
    [SerializeField] private BreadThrowPathRaycast throwPathRaycast;
    [Tooltip("줌 유지 시간")]
    Coroutine aimingKeepTimeCoroutine;
    [SerializeField] private float aimingKeepTime;
    [SerializeField] private float curKeepTime;
    [Tooltip("쿨타임")]
    [SerializeField] private float throwCooldownTime;
    [Tooltip("쿨타임 여부")]
    [SerializeField] private bool isCooldown = false;
    [Tooltip("빵 재장전 시간")]
    [SerializeField] private float reloadTime;

    void Awake()
    {
        SetBread();
    }

    //시작 시점에 빵 갯수 초기화  
    void Start()
    {
        CreateBread(1);
        weaponHandlerAni.SetFloat("ReloadSpeed", 2f);
    }

    #region 빵 사용 관련

    /// <summary>
    /// 빵 충전/보급
    /// </summary>
    public void SupplyBread()
    {
        Managers.Player.PlayerStat.Bread = Managers.Player.PlayerStat.MaxBread;
    }

    /// <summary>
    /// 빵 최대 보유 갯수 증가
    /// </summary>
    /// <param name="amount">증가량</param>
    public void UpgradeMaxBread(int amount)
    {
        Managers.Player.PlayerStat.MaxBread += amount;
    }

    /// <summary>
    /// 빵 생성 (초기화 및 애니메이션에서 호출)
    /// </summary>
    /// <param name="type"></param>
    public void CreateBread(int type)
    {
        if (Managers.Player.PlayerStat.Bread <= 0)
        {
            return;
        }

        if (type == 0)
            Managers.Player.PlayerStat.Bread--;

        //빵 프리팹 생성
        onHandBread = Instantiate(BreadPrefs, transform);
        //빵 위치 조정
        onHandBread.transform.localPosition = new Vector3(0.62f, 0.2f, 0.7f);
        onHandBread.transform.localRotation = Quaternion.identity;
        onHandBread.transform.localScale = Vector3.one;
    }

    public void SetBread()
    {
        if (Managers.Player.PlayerStat.Bread == 0)
            camController.CameraAim(false);
    }

    #endregion

    #region 빵 공격 관련
    /// <summary>
    /// 애니메이션에서 호출할 근접 시작 알림
    /// </summary>
    public void StartMeleeAttack() => onHandBread.StartSwingBaguette();

    /// <summary>
    /// 애니메이션에서 호출할 근접 공격 종료 알림
    /// </summary>
    public void EndMeleeAttack() => onHandBread.EndSwingBaguette();

    /// <summary>
    /// 빵 던지기
    /// </summary>
    public void ThrowBread()
    {
        if (aimingKeepTimeCoroutine != null)
        {
            StopCoroutine(aimingKeepTimeCoroutine);
            aimingKeepTimeCoroutine = null;
        }

        if (!Managers.Player.PlayerStat.Abilities.Contains(Ability.RapidThrow)
            && (isCooldown || (curKeepTime < aimingKeepTime)
            || Managers.Player.PlayerStat.Bread < 1))
        {
            camController.CameraAim(false);
            RemoveThrowPath();
            return;
        }

        if (!weaponHandlerAni.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        isCooldown = true;
        //발사 각도 전달하기
        onHandBread.SetFireAngle(fireAngleTransform.forward);
        //빵 던지기
        onHandBread.ThrowBaguette();
        onHandBread = null;
        //빵 재장전
        weaponHandlerAni.Play("ReloadBaguette");
        //시간 측정
        StartCoroutine(ThrowCooldown());
        StartCoroutine(ThrowBreadCoroutine());
    }

    /// <summary>
    /// 던지기 쿨타임 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsCooldown()
    {
        return isCooldown;
    }

    public void ShowThrowPath()
    {
        throwPathRaycast.DrowThrowPath();
    }

    /// <summary>
    /// 내부에서 사용할 경로 제거
    /// </summary>
    void RemoveThrowPath()
    {
        throwPathRaycast.HideThrowPath();
    }

    #endregion

    public void StartAimingTime()
    {
        if (aimingKeepTimeCoroutine != null)
            return;

        aimingKeepTimeCoroutine = StartCoroutine(CheckAimingTime());
    }

    #region 코루틴 (시간 측정)
    /// <summary>
    /// 던지기 쿨타임 코루틴
    /// </summary>
    IEnumerator ThrowCooldown()
    {

        float curCoolTime = 0;
        while (curCoolTime < throwCooldownTime)
        {
            curCoolTime++;
            yield return new WaitForSeconds(1.2f);
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
        if (!Managers.Player.PlayerStat.Abilities.Contains(Ability.RapidThrow))
            camController.CameraAim(false);
        yield return null;
        RemoveThrowPath();
    }

    IEnumerator CheckAimingTime()
    {
        curKeepTime = 0.0f;
        float interval = 0.1f;

        //재장전 시간 동안 조준 상태 유지
        while (true)
        {
            curKeepTime += interval;
            yield return new WaitForSeconds(interval);
        }
    }
    #endregion
}
