using UnityEngine;

public class Baguette : MonoBehaviour
{
    [Header("빵 컨포넌트")]
    public Animator breadAni;
    public Rigidbody breadRigid;
    [Tooltip("시각화 빵")]
    public GameObject breadVisual;
    [Header("빵 상태")]
    bool isFired = false;
    bool isStuck = false;
    [SerializeField] private float flySpeed;
    public float explodeTime = 1.5f;
    [SerializeField] private Vector3 fireAngle;

    void Awake()
    {
        breadRigid.isKinematic = true;
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    void Update()
    {
        if(isFired && !isStuck)
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    public void ThrowBaguette()
    {
        transform.SetParent(null);
        isFired = true;
        breadAni.Play("Rotate");

        //일정 시간 비행 후 자동 삭제
        if(gameObject.activeSelf && !isStuck)
            Destroy(gameObject, explodeTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);   
        }
        else if (collision.gameObject.CompareTag("Enemy")){
            
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            isFired = true;
            SetStuck();
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
    public void SetStuck()
    {
        isStuck = true;
        //애니메이션 정지
        breadAni.enabled = false;
        //고정 각도 설정
        breadVisual.transform.localRotation = Quaternion.identity;
        //땅 태그 할당
        gameObject.tag = "Ground";
    }
    #endregion
}
