using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : Poolable
{
    [Tooltip("플레이어")]
    [SerializeField] private PlayerController player;
    [Header("적 컨포넌트")]
    [SerializeField] Rigidbody enemyRigid;
    [Tooltip("적 상태")]
    [SerializeField] float maxHP;
    [SerializeField] float curHP;
    [SerializeField] private float _moveSpeed = 10f;

    private Rigidbody _rb;

    public UnityEvent<int> onPlayerDamaged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHP = maxHP;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        _rb = gameObject.GetorAddComponent<Rigidbody>();
        onPlayerDamaged = new UnityEvent<int>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (OutOfBounds())
            Managers.Resource.Destroy(gameObject);
    }

    #region 플레이어 몸체와 접촉 - 피해 여부 판단

    /// <summary>
    /// 몸체 접촉 감지
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerTemp")
        {
            player.TakeDamage(1);
            Debug.Log("플레이어가 적에게 피해를 입었습니다. 현재 체력: " + player.GetCurrentHealth());
            StartCoroutine(StillTriggeredCoroutine());
            Debug.Log("Coroutine 시작");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerTemp")
        {
            Debug.Log("플레이어가 적과 접촉을 끊었습니다.");
            // StopCoroutine(StillTriggeredCoroutine());
            StopAllCoroutines();
        }
    }
    /// <summary>
    /// 적 피격, 최대 HP: 3
    /// </summary>
    /// <param name="damage"></param>
    public void EnemyHit(float damage)
    {
        curHP -= damage;
        //피격 액션 넣기
        enemyRigid.AddForce(Vector3.forward * 10.0f, ForceMode.Impulse);
        enemyRigid.AddForce(Vector3.up * 12.0f, ForceMode.Impulse);
        if (curHP <= 0)
        {
            //적 사망 액션(?)
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 최초 접촉 후 1초마다 계속 trigger 상태인지 확인해서 피해를 입히는 로직
    /// </summary>
    IEnumerator StillTriggeredCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            if (player != null && player.GetCurrentHealth() > 0)
            {
                player.TakeDamage(1);
                Debug.Log("플레이어가 계속 적에게 피해를 입습니다. 현재 체력: " + player.GetCurrentHealth());
            }
        }
    }

    #endregion

    #region 플레이어 추적
    private void FollowPlayer()
    {
        Vector3 delta = player.transform.position - transform.position;
        if (delta.magnitude < 1f)
        {
            return;
        }
        float step = _moveSpeed * Time.deltaTime;
        Vector3 nextPos = Vector3.MoveTowards(transform.position, player.transform.position, step);
        delta.y = 0;
        Quaternion rotation = Quaternion.LookRotation(delta, Vector3.up);

        _rb.MovePosition(nextPos);
        _rb.MoveRotation(rotation);
    }
    #endregion

    private bool OutOfBounds()
    {
        return transform.position.x < -75 || transform.position.x > 75
        || transform.position.z < -75 || transform.position.z > 75;
    }
}