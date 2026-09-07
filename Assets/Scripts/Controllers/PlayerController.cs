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
    [Tooltip("특수 공격 키")]
    public InputAction skillInput;
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
    public float WalkSpeed = 10f;
    public UnityEvent PlayerDied;

    [Header("상호작용 할 가게")]
    [SerializeField] private ShopKeeper _shop;
    [SerializeField] private Patissier _bread;
    public HashSet<CarController> Cars = new();

    private bool _inputEnabled = true;
    private Rigidbody _rb;

    private Vector3 _stormTargetPos;
    private GameObject _blastGuide;
    private MeshFilter _blastGuideMeshFilter;
    private MeshRenderer _blastGuideMeshRenderer;
    private Mesh _blastGuideMesh;

    private const int BlastGuideSegments = 64;

    public CamController CamController { get { return _camController; } set { _camController = value; } }
    public bool InputEnabled
    {
        get { return _inputEnabled; }
        set
        {
            _inputEnabled = value;
            if (value)
            {
                EnableInputAction();
            }
            if (!value)
            {
                DisableInputAction();
                StartCoroutine(EnableInputAfterDelay(0.5f));
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

    /// <summary>
    /// InputAction 활성화
    /// </summary>
    void EnableInputAction()
    {
        moveInput.Enable();
        jumpInput.Enable();
        interactionInput.Enable();
        skillInput.Enable();
    }

    void DisableInputAction()
    {
        moveInput.Enable();
        jumpInput.Enable();
        interactionInput.Enable();
        skillInput.Enable();
    }

    void Awake()
    {
        Managers.Player.PlayerController = this;
        _rb = gameObject.GetorAddComponent<Rigidbody>();

        InitBlastGuide();
        HideBlastGuide();
    }

    void Update()
    {
        //사망 시 입력 무시
        if (isDead)
            return;
        JumpPlayer();
        InteractionWithOthers();
        UseSkill();

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

        _rb.MovePosition(_rb.position + direction * WalkSpeed * Time.fixedDeltaTime);
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

    private void UseSkill()
    {
        bool butterBlast = Managers.Player.PlayerStat.Abilities.Contains(Ability.ButterBlast) && Managers.Player.PlayerStat.ButterAmount > 0;
        if (!butterBlast)
            return;
        Vector3 camForward = CamController.transform.forward;
        Vector3 targetPos = transform.position + camForward.normalized * 6f;
        targetPos.y = 0.05f;
        _stormTargetPos = targetPos;

        if (skillInput.IsPressed())
            ShowBlastGuide(_stormTargetPos, 5f);
        if (skillInput.WasReleasedThisFrame())
        {
            Managers.Player.PlayerStat.ButterAmount--;
            GameObject blastGuide = DuplicateBlastGuide();
            HideBlastGuide();
            StartCoroutine(ButterBlast(_stormTargetPos, blastGuide));
        }
    }

    private IEnumerator ButterBlast(Vector3 stormTargetPos, GameObject blastGuide)
    {
        float elapsed = 0f;

        while (elapsed < 5f)
        {
            SummonBread(stormTargetPos);

            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }

        Destroy(blastGuide);
    }

    private void SummonBread(Vector3 stormTargetPos)
    {
        Baguette baguette = Managers.Resource.Instantiate("Players/Baguette").GetorAddComponent<Baguette>();
        Vector2 randomCircle = Random.insideUnitCircle * 4f;
        Vector3 randomPos = stormTargetPos + new Vector3(
            randomCircle.x,
            10f,
            randomCircle.y
        );
        baguette.transform.position = randomPos;
        baguette.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        baguette.SetFireAngle(new Vector3(0, -1, 0));
        baguette.ThrowBaguette();
    }
    #endregion

    #region 플레이어 체력 변동 & 사망 by.Jaehoon
    /// <summary>
    /// 플레이어가 피해를 입었을 때 체력 감소
    /// </summary>
    /// <param name="damage">받은 피해량</param>
    public void TakeDamage(int damage)
    {
        Managers.Player.AcquireHp(-damage);
        if (Managers.Player.PlayerStat.Hp < 0)
            Managers.Player.PlayerStat.Hp = 0;

        if (Managers.Player.PlayerStat.Hp <= 0)
            Die();
    }
    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    private void Die()
    {
        isDead = true;
        PlayerDied.Invoke();
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

    private void InitBlastGuide()
    {
        Transform blastGuideTransform = transform.Find("BlastGuide");

        _blastGuide = blastGuideTransform.gameObject;
        _blastGuideMeshFilter = _blastGuide.GetorAddComponent<MeshFilter>();
        _blastGuideMeshRenderer = _blastGuide.GetorAddComponent<MeshRenderer>();

        _blastGuideMesh = new Mesh();
        _blastGuideMeshFilter.mesh = _blastGuideMesh;
    }

    private void DrawBlastGuide(Vector3 worldCenter, float radius)
    {
        Vector3[] vertices = new Vector3[BlastGuideSegments + 1];
        int[] triangles = new int[BlastGuideSegments * 3];

        vertices[0] = _blastGuide.transform.InverseTransformPoint(worldCenter);

        for (int i = 0; i < BlastGuideSegments; i++)
        {
            float angle = 2f * Mathf.PI * i / BlastGuideSegments;

            Vector3 worldPosition = worldCenter + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            vertices[i + 1] =
                _blastGuide.transform.InverseTransformPoint(worldPosition);
        }

        for (int i = 0; i < BlastGuideSegments; i++)
        {
            int current = i + 1;
            int next = (i + 1) % BlastGuideSegments + 1;

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = next;
            triangles[i * 3 + 2] = current;
        }

        _blastGuideMesh.Clear();
        _blastGuideMesh.vertices = vertices;
        _blastGuideMesh.triangles = triangles;
        _blastGuideMesh.RecalculateNormals();
        _blastGuideMesh.RecalculateBounds();
    }

    private void ShowBlastGuide(Vector3 center, float radius)
    {
        DrawBlastGuide(center, radius);
        _blastGuide.SetActive(true);
    }

    private void HideBlastGuide()
    {
        _blastGuide.SetActive(false);
    }

    private GameObject DuplicateBlastGuide()
    {
        GameObject blastGuide = Instantiate(
            _blastGuide,
            _blastGuide.transform.parent
        );

        blastGuide.transform.SetParent(null, true);

        MeshFilter meshFilter = blastGuide.GetComponent<MeshFilter>();
        meshFilter.mesh = Instantiate(_blastGuideMesh);

        MeshRenderer meshRenderer = blastGuide.GetComponent<MeshRenderer>();

        Color butterColor = meshRenderer.material.color;
        butterColor.a = 0.1f;
        meshRenderer.material.color = butterColor;

        return blastGuide;
    }
}
