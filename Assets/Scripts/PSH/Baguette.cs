using UnityEngine;

public class Baguette : MonoBehaviour
{
    public Animator breadAni;
    public Rigidbody breadRigid;

    public float explodeTime = 1.5f;
    /// <summary>
    /// 빵 날리기 
    /// </summary>
    public void ThrowBaguette(float force)
    {
        Debug.Log("빵 발사됨");
        transform.SetParent(null);
        breadRigid.AddForce(transform.forward * force, ForceMode.Force);
        breadAni.Play("Rotate");

        //일정 시간 비행 후 자동 삭제
        Destroy(gameObject, explodeTime);
    }
}
