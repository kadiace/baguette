using UnityEngine;

public class Baguette : MonoBehaviour
{
    public Animator breadAni;
    public Rigidbody breadRigid;
    /// <summary>
    /// 빵 날리기 
    /// </summary>
    public void ThrowBaguette(float force)
    {
        Debug.Log("빵 발사됨");
        breadRigid.AddForce(transform.forward * force, ForceMode.Force);
        breadAni.Play("Rotate");
    }
}
