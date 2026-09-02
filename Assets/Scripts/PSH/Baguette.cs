using UnityEngine;
using System.Collections;

public class Baguette : MonoBehaviour
{
    [Header("빵 컨포넌트")]
    public Animator breadAni;
    public Rigidbody breadRigid;
    [Tooltip("빵 날리기용 빵")]
    public GameObject breadForVisual;
    [Tooltip("빵 설치용 빵")]
    [SerializeField] GameObject breadForStuck;
    [SerializeField] Transform breadForStuckVisual;
    [Header("빵 속성")]
    [Tooltip("근접 공격 시 할당할 데미지")]
    [SerializeField] float meleeDamage = 1f;
    [Tooltip("투척 공격 시 할당할 데미지")]
    [SerializeField] float throwDamage = 3f;
    [Tooltip("현재 할당된 데미지 (기본: 근접 공격)")]
    [SerializeField] float curDamage;
    [Header("현재 빵 상태 (공격, 투척, 부착)")]
    bool isAttack;
    bool isThrow = false;
    bool isStuck = false;
    [Tooltip("공격 상태 (휘두르기 || 던지기)")]
    [SerializeField] private float flySpeed;
    public float explodeTime = 1.5f;
    [SerializeField] private Vector3 fireAngle;

    void Awake()
    {
        isAttack = false;
        breadRigid.isKinematic = true;
        breadForStuck.SetActive(false);
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    void Update()
    {
        if (isThrow && !isStuck)
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
        if (isStuck)
            Debug.Log("빵이 벽에 고정됨");
    }

    #region 공격 관련 (근접 공격, 던지기)
    /// <summary>
    /// 공격 상태 ON (근접용)
    /// </summary>
    public void StartSwingBaguette() => isAttack = true;
    /// <summary>
    /// 공격 상태 OFF (근접용)
    /// </summary>
    public void EndSwingBaguette() => isAttack = false;
    /// <summary>
    /// 빵 날리기 (독립, 회전, 자폭 시작)
    /// </summary>
    public void ThrowBaguette()
    {
        // Change CapsuleCollider
        CapsuleCollider capsule = gameObject.GetorAddComponent<CapsuleCollider>();
        capsule.height = 1.15f;
        capsule.direction = 1;
        capsule.center = Vector3.zero;

        SphereCollider sphere = gameObject.GetorAddComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = 0.75f;
        sphere.center = Vector3.zero;

        curDamage = throwDamage;
        isThrow = true;
        isAttack = true;
        transform.SetParent(null); //독립시키기
        //회전 애니메이션 실행
        breadAni.Play("Rotate");
        //일정 시간 비행 후 자동 삭제
        StartCoroutine(DestroyAfterTime());
    }
    #endregion

    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Floor")) && isThrow)
        {
            Debug.Log("빵 고정");
            SetStuck(collision);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //플레이어와의 접촉에 의한 공격 방지 장치
            if (!isAttack)
            {
                return;
            }

            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (isThrow)
            {
                curDamage = throwDamage;
                //몬스터 접근 후 데미지 주기
                //빵을 던져서 맞출 경우, 빵의 position 값을 전달
                enemy.EnemyHit(curDamage, transform.position);
            }
            else
            {
                curDamage = meleeDamage;
                enemy.EnemyHit(curDamage, transform.root.position);
            }
        }
        /*
        else if (collision.gameObject.CompareTag("Ground") && isThrow)
        {
            Debug.Log("빵이 땅에 닿음");
            //Destroy(gameObject);
        }
        */
    }
    #region 빵 고정 관련
    public void SetFireAngle(Vector3 forwardDirection)
    {
        if (forwardDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(forwardDirection);
        }
    }

    /// <summary>
    /// 벽에 고정시키기
    /// </summary>
    public void SetStuck(Collider wallCollider)
    {
        //던지기용 빵 자폭 멈추기
        StopAllCoroutines();
        isStuck = true;

        //던진 각도
        Quaternion actualRotatingVisualRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);

        //Raycast로 박힌 위치 및 벽 수직 회전 계산
        Ray ray = new Ray(transform.position - transform.forward * 1.5f, transform.forward);
        RaycastHit hit;
        Vector3 stickPosition = transform.position;
        Quaternion wallPerpendicularRotation = transform.rotation;

        if (wallCollider.Raycast(ray, out hit, 3.0f))
        {
            float stickOutDistance = 0.3f;
            stickPosition = hit.point + (hit.normal * stickOutDistance);

            Quaternion lookWall = Quaternion.LookRotation(-hit.normal, Vector3.up);

            wallPerpendicularRotation = lookWall * Quaternion.Euler(90f, 0f, 0f);
        }

        //물리 발판 세팅
        breadForStuck.transform.SetParent(null);
        breadForStuck.transform.position = stickPosition;
        // breadForStuck.transform.rotation = wallPerpendicularRotation;
        breadForStuck.transform.rotation = actualRotatingVisualRotation;

        //시각 발판 각도 설정
        if (breadForStuckVisual != null)
        {
            breadForStuckVisual.rotation = actualRotatingVisualRotation *
                Quaternion.Euler(Random.Range(-3f, 3f), 0f, 0f);
        }

        breadForStuck.SetActive(true);

        Destroy(gameObject);
    }

    #endregion

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(explodeTime);
        Destroy(gameObject);
    }
}
