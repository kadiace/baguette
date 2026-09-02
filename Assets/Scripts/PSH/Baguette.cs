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
    [Header("빵 상태")]
    bool isThrow = false;
    bool isStuck = false;
    [Tooltip("공격 상태 (휘두르기 || 던지기)")]
    [SerializeField] private float flySpeed;
    public float explodeTime = 1.5f;
    [SerializeField] private Vector3 fireAngle;

    void Awake()
    {
        //Time.timeScale = 0.3f;    //테스트용:
        breadRigid.isKinematic = true;
        breadForStuck.SetActive(false);
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    void Update()
    {
        if(isThrow && !isStuck)
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
        if(isStuck)
            Debug.Log("빵이 벽에 고정됨");
    }

    /// <summary>
    /// 빵 날리기 (독립, 회전, 자폭 시작)
    /// </summary>
    public void ThrowBaguette()
    {
        isThrow = true;
        //독립시키기
        transform.SetParent(null);
        //회전 애니메이션 실행
        breadAni.Play("Rotate");
        //일정 시간 비행 후 자동 삭제
        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isThrow)
        {
            Debug.Log("빵이 땅에 닿음");
            Destroy(gameObject);   
        }
        else if (collision.gameObject.CompareTag("Enemy")){
            //TODO: 적에게 피해주기
        }
        else if (collision.gameObject.CompareTag("Wall") && isThrow)
        {
            Debug.Log("빵이 벽에 닿음");
            SetStuck(collision);
        }
    }
    #region 빵 고정 관련
    public void SetFireAngle(Vector3 forwardDirection){
        if (forwardDirection != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(forwardDirection);
        }
    }

    /// <summary>
    /// 벽에 고정시키기
    /// </summary>
    public void SetStuck(Collider wallCollider)
    {   //던지기용 빵 자폭 멈추기
        StopAllCoroutines();    
        isStuck = true;

        //Raycast로 박힌 위치 계산
        Ray ray = new Ray(transform.position - transform.forward * 1.5f, transform.forward);
        RaycastHit hit;
        Vector3 stickPosition = transform.position;

        if (wallCollider.Raycast(ray, out hit, 3.0f)){
        float stickOutDistance = 0.3f; 
        stickPosition = hit.point + (hit.normal * stickOutDistance);
        }

    // 2. 발판용 빵 세팅
    breadForStuck.transform.SetParent(null);
    breadForStuck.transform.position = stickPosition;


        //발판용 빵 설치하기
        breadForStuck.transform.SetParent(null);
        breadForStuck.SetActive(true);
        //발판은 벽에 고정되어야 하므로 Rigidbody를 Kinematic으로 고정
        Rigidbody stuckRigid = breadForStuck.GetComponent<Rigidbody>();
        if (stuckRigid != null)
        {
            stuckRigid.isKinematic = true;
        }

        //던지기용 빵 비활성화
        Destroy(gameObject);
    }
    #endregion

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(explodeTime);
        Destroy(gameObject);
    }
}
