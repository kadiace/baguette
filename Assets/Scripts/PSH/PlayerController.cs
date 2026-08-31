using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("카메라")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CamController camController;
    [Header("플레이어 무기 관리자(?)")]
    public WeaponHandler weaponHandler;
    [Header("플레이어 조작")]
    [SerializeField] Rigidbody pRigid;
    [Tooltip("W A S D")]
    public InputAction moveInput;
    [Tooltip("Space")]
    public InputAction jumpInput;
    [Tooltip("근접공격 애니메이션")]
    public Animator meleeAni;
    public bool isThrowReady = false;
    /*
    [Tooltip("마우스 좌 우")]
    public InputAction mouseInput;
    */
    [SerializeField] private float rightClickTime = 0f;
    [Tooltip("조준을 위한 우클릭 유지 시간")]
    [SerializeField] private float aimTime = 1.5f;

    public bool isDead = false;
    [SerializeField] float walkSpeed;
    [Tooltip("마우스 좌 우 회전 속도(카메라 회전 속도)")]
    [SerializeField] float rotateSpeed;
    [Tooltip("플레이어 이동 방향")]
    [SerializeField] Vector2 movePos;
    [SerializeField] float jumpHeight;

    private void Awake(){
        pRigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InitInputAction();
    }

    /// <summary>
    /// InputAction 활성화
    /// </summary>
    void InitInputAction()
    {
        moveInput.Enable();
        jumpInput.Enable();
        //mouseInput.Enable();
    }

    void Update(){
        CheckKeyboardInput();
        RotatePlayer();
    }

    /// <summary>
    ///  InputAction 감지
    /// </summary>
    void CheckKeyboardInput()
    {
        //사망 시 입력 무시
        if (isDead) 
            return;
        MovePlayer();
        JumpPlayer();
        AttackPlayer();
    }

    #region 플레이어 조작(회전, 이동, 회전, 공격)

    /// <summary>
    /// 마우스 화전에 따른 플레이어 회전, 추후 카메라 회전과 연동으로 변경 필요
    /// </summary>
    void RotatePlayer()
    {
        float mouseXInput = Mouse.current.delta.x.ReadValue() * rotateSpeed * Time.deltaTime;
        //플레이어 회전
        transform.Rotate(Vector3.up * mouseXInput);
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    void MovePlayer()
    {
        movePos = moveInput.ReadValue<Vector2>();

        if (movePos.sqrMagnitude > 0.001f)
        {
            //카메라 시선 방향 확인
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            //플레이어 이동 위치 설정 및 이동
            Vector3 movementDirection = (camForward * movePos.y) + (camRight * movePos.x);
            transform.Translate(movementDirection * Time.deltaTime * walkSpeed, Space.World);
        }
    }

    /// <summary>
    /// 플레이어 점프
    /// </summary>
    void JumpPlayer()
    {
        if(jumpInput.triggered)
            pRigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    /// <summary>
    /// 플레이어 공격
    /// </summary>
    void AttackPlayer()
    {
        //좌 "클릭"
        if (Input.GetMouseButtonDown(0)){
            if(isThrowReady)
                return;
            weaponHandler.MeleeAttack();
            meleeAni.Play("SwingDiagonal");
        }
        //우 "클릭"
        else if (Input.GetMouseButton(1))
        {
            rightClickTime += Time.deltaTime;
            if (rightClickTime >= aimTime){
                isThrowReady = true;
                camController.CameraAim(true);
            }
            
        }
        // 우클릭 해제 시 카메라 줌아웃
        else if (Input.GetMouseButtonUp(1))
        {
            weaponHandler.ThrowBread();
            rightClickTime = 0f;
            isThrowReady = false;
        }
    }

    #endregion

    /// <summary>
    /// 충돌 감지
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

    }
}
