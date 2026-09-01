using UnityEngine;

public class Baguette : MonoBehaviour
{
    public Animator breadAni;
    public Rigidbody breadRigid;
    bool isFired = false;
    bool isStair = false;
    [SerializeField] private float flySpeed;
    public float explodeTime = 1.5f;

    void Awake()
    {
        breadRigid.isKinematic = true;
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    void Update()
    {
        if(isFired)
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
    }

    /// <summary>
    /// 빵 날리기 
    /// </summary>
    public void ThrowBaguette()
    {
        //Debug.Log("빵 발사됨");
        transform.SetParent(null);
        isFired = true;
        breadAni.Play("Rotate");

        //일정 시간 비행 후 자동 삭제
        if(gameObject.activeSelf && !isStair)
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
            //TODO: 벽에 박히기
        }
    }
}
