using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("카메라")]
    [SerializeField] private CamController _camController;
    [Header("플레이어 무기 관리자(?)")]
    public WeaponHandler weaponHandler;
    [Header("플레이어 조작")]
    [Tooltip("W A S D")]
    public InputAction moveInput;
    [Tooltip("Space")]
    public InputAction jumpInput;
    [Tooltip("상호작용 키")]
    public InputAction interactionInput;
    [Tooltip("공격 애니메이션(웨폰 헨들러)")]
    public Animator weaponHandlerAni;
    public bool isThrowReady = false;
    [SerializeField] private float rightClickTime = 0f;
    [Tooltip("조준을 위한 우클릭 유지 시간")]
    [SerializeField] private float aimTime;
    [Tooltip("플레이어 이동 방향")]
    [SerializeField] Vector2 movePos;
    [SerializeField] float jumpHeight;

    #region 플레이어 상태값 by.Jeehoon
    [Header("플레이어 상태")]
    public bool isDead = false;
    public bool isGround = true;
    [SerializeField] float walkSpeed;
    [Tooltip("플레이어 최대 체력")]
    [SerializeField] private int maxHealth = 5;
    [Tooltip("플레이어 현재 체력")]
    [SerializeField] private int currentHealth = 5;
    [Tooltip("플레이어 체력 변동 이벤트")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent PlayerDied;

    [Header("상호작용 할 가게")]
    [SerializeField] private ShopKeeper _shop;
    [SerializeField] private Patissier _bread;
    public HashSet<CarController> Cars = new();

    private bool _inputEnabled = true;
    private Rigidbody _rb;

    public CamController CamController { get { return _camController; } set { _camController = value; } }
    public bool InputEnabled
    {
        get { return _inputEnabled; }
        set
        {
            _inputEnabled = value;
            if (value)
            {
                moveInput.Enable();
                jumpInput.Enable();
                interactionInput.Enable();
            }
            if (!value)
            {
                moveInput.Disable();
                jumpInput.Disable();
                interactionInput.Disable();
                StartCoroutine(EnableInputAfterDelay(1f));
            }
        }
    }

    #endregion

    /// <summary>
    /// 컨포넌트 할당 시 자동으로 변수 값 할당
    /// </summary>
    void Reset()
    {
        _rb = GetComponent<Rigidbody>();
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
        interactionInput.Enable();
    }

    void Awake()
    {
        Managers.Player.PlayerController = this;
        _rb = gameObject.GetorAddComponent<Rigidbody>();
    }

    void Update()
    {
        //사망 시 입력 무시
        if (isDead)
            return;
        JumpPlayer();
        InteractionWithOthers();

        if (!Managers.Player.PlayerStat.Abilities.Contains(Ability.RapidThrow))
        {
            AttackPlayer();
            return;
        }

        // Rapid Throw
        if (Managers.Player.PlayerStat.Bread <= 0)
        {
            _camController.CameraAim(false);
            return;
        }
        _camController.CameraAim(true);
        RapidThrow();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // [-70, 70] 내부에 있도록 보정
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -70f, 70f);
        pos.z = Mathf.Clamp(pos.z, -70f, 70f);

        transform.position = pos;
    }

    #region 플레이어 조작(이동, 공격, 상호작용)  *회전은 카메라에서 조절
    /// <summary>
    /// 플레이어 이동 (Rigid 사용)
    /// </summary>
    void MovePlayer()
    {
        movePos = moveInput.ReadValue<Vector2>();

        if (movePos.sqrMagnitude < 0.001f)
            return;

        // 기존에 있던 수평 속도 제거
        Vector3 currentVel = _rb.linearVelocity;
        currentVel.x = 0;
        currentVel.z = 0;
        _rb.linearVelocity = currentVel;

        //카메라 시선 방향 확인
        Vector3 camForward = _camController.transform.forward;
        Vector3 camRight = _camController.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        //플레이어 이동 위치 설정 및 이동
        Vector3 direction = (camForward * movePos.y) + (camRight * movePos.x);

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
            return;

        _rb.MovePosition(_rb.position + direction * walkSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 플레이어 점프
    /// </summary>
    void JumpPlayer()
    {
        if (!isGround)
            return;

        if (jumpInput.triggered)
        {
            _rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGround = false;
        }
    }

    /// <summary>
    /// 플레이어 공격
    /// </summary>
    void AttackPlayer()
    {
        if (Input.GetMouseButtonDown(0))
            Swing();
        else if (Input.GetMouseButton(1))
            ThrowReady();
        else if (Input.GetMouseButtonUp(1))
            Throw();
    }

    private void Swing()
    {
        if (isThrowReady || weaponHandler.IsCooldown())
            return;
        weaponHandlerAni.Play("SwingDiagonal");
    }

    private void ThrowReady()
    {
        rightClickTime += Time.deltaTime;
        if (rightClickTime >= aimTime)
        {
            if (Managers.Player.PlayerStat.Bread < 1)
                return;
            isThrowReady = true;
            _camController.CameraAim(true);
            weaponHandler.ShowThrowPath();
            weaponHandler.StartAimingTime();

        }
    }

    private void Throw()
    {
        weaponHandler.ThrowBread();
        rightClickTime = 0f;
        isThrowReady = false;
    }

    private void RapidThrow()
    {
        if (Input.GetMouseButtonDown(0))
            weaponHandler.ShowThrowPath();
        if (Input.GetMouseButtonUp(0))
            weaponHandler.ThrowBread();
    }

    /// <summary>
    /// 상호작용 함수 (F 키)
    /// </summary>
    void InteractionWithOthers()
    {
        if (!interactionInput.triggered)
            return;

        float distShop = (transform.position - _shop.transform.position).magnitude;
        float distBread = (transform.position - _bread.transform.position).magnitude;

        float distCar = float.PositiveInfinity;
        CarController car = null;

        foreach (CarController curCar in Cars)
        {
            float curDistCar = (transform.position - curCar.transform.position).magnitude;
            if (curDistCar < distCar)
            {
                car = curCar;
                distCar = curDistCar;
            }
        }

        float interactDistance = 4f;

        if (distShop <= distBread
            && distShop <= distCar
            && distShop <= interactDistance)
        {
            _shop.ShowStore();
            return;
        }
        else if (distBread <= distCar
            && distBread <= interactDistance)
        {
            weaponHandler.SupplyBread();
            return;
        }
        else if (car != null
            && distCar <= interactDistance
            && Managers.Player.PlayerStat.Abilities.Contains(Ability.Carjack))
        {
            car.Ride(this);
            return;
        }

        if (Managers.Player.PlayerStat.Abilities.Contains(Ability.RemoteSupply))
            weaponHandler.SupplyBread();
    }

    #endregion

    #region 플레이어 체력 변동 & 사망 by.Jaehoon
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
        PlayerDied.Invoke();
    }
    /// <summary>
    /// 현재 체력 반환
    /// </summary>
    /// <returns>현재 체력</returns>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncreaseHealth()
    {
        SetCurrentHealth(1);
    }

    public void SetCurrentHealth(int count)
    {
        currentHealth += count;
        HealthEventInvoke();
    }

    /// <summary>
    /// 최대 체력 반환
    /// </summary>
    /// <returns>최대 체력</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    /// <summary>
    /// 최대 체력 설정. 최대 체력 변경 시 현재 체력도 최대치로 초기화
    /// </summary>
    /// <param name="newMaxHealth"></param>
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth; // 체력도 최대치로 초기화
        HealthEventInvoke();
    }
    /// <summary>
    /// 플레이어의 이동속도 전달
    /// </summary>
    /// <returns></returns>
    public float GetPlayerSpeed()
    {
        return walkSpeed;
    }

    /// <summary>
    /// 플레이어 속도 설정
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetPlayerSpeed(float newSpeed)
    {
        walkSpeed = newSpeed;
    }

    #endregion

    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _inputEnabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Floor"))
            isGround = true;
    }

}
