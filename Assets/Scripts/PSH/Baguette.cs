using UnityEngine;

public class Baguette : MonoBehaviour
{
    public Animator breadAni;
    bool isFired = false;
    [SerializeField] private float flySpeed;
    public float explodeTime = 1.5f;

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
        Destroy(gameObject, explodeTime);
    }
}
