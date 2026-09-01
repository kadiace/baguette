using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class JHTmpPlayerController : MonoBehaviour
{
    [Tooltip("카메라")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CamController camController;
    [Header("제훈 임시 플레이어 무기 관리자(?)")]
    public JHTmpWeaponHandler JHTmpWeaponHandler;
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

    [Tooltip("플레이어 최대 체력")]
    [SerializeField] private int maxHealth = 5;
    [Tooltip("플레이어 현재 체력")]
    [SerializeField] private int currentHealth = 5;
    [Tooltip("플레이어 체력 변동 이벤트")]
    public UnityEvent<int> OnHealthChanged;

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
            JHTmpWeaponHandler.MeleeAttack();
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
            JHTmpWeaponHandler.ThrowBread();
            rightClickTime = 0f;
            isThrowReady = false;
        }
    }

    #endregion

    #region 플레이어 체력 변동 & 사망
    /// <summary>
    /// 현재 체력 변동 이벤트를 Invoke합니다.
    /// </summary>
    private void HealthEventInvoke()
    {
        OnHealthChanged.Invoke(currentHealth);
    }
    /// <summary>
    /// 플레이어가 피해를 입었을 때 체력 감소
    /// </summary>
    /// <param name="damage">받은 피해량</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;            
        }

        HealthEventInvoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    private void Die()
    {
        isDead = true;
        Debug.Log("플레이어 사망");
    }
    /// <summary>
    /// 현재 체력 반환
    /// </summary>
    /// <returns>현재 체력</returns>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>최대 체력</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    #endregion

    /// <summary>
    /// 트리거 발동 감지
    /// </summary>
    /// <param name="other"></param>
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.name == "EnemyTemp")
    //     {
    //         TakeDamage(1);
    //         Debug.Log("플레이어가 적에게 피해를 입었습니다. 현재 체력: " + currentHealth);
    //     }
    // }
}
