using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : Poolable
{
    [Header("적 컨포넌트")]
    [SerializeField] Rigidbody enemyRigid;
    [Tooltip("적 상태")]
    [SerializeField] float maxHP;
    [SerializeField] float curHP;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float deadDelay;

    private Rigidbody _rb;

    public UnityEvent<int> onPlayerDamaged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHP = maxHP;
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
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"충돌 감지: {other.gameObject}");
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Player.PlayerController.TakeDamage(1);
            StartCoroutine(StillTriggeredCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // StopCoroutine(StillTriggeredCoroutine());
            StopAllCoroutines();
        }
    }
    /// <summary>
    /// 적 피격, 최대 HP: 3
    /// </summary>
    /// <param name="damage"></param>
    public void EnemyHit(float damage, EnemyHitCause enemyHitCause, Vector3? attackerPos = null)
    {
        curHP -= damage;
        //피격 액션 넣기
        if (attackerPos != null)
        {
            //피격 방향 계산하기
            Vector3 knockbackDir = transform.position - attackerPos.Value; //벡터 계산
            knockbackDir.y = 0f;        //급격 넉백 방지
            knockbackDir.Normalize();   //정규화

            //피격 방행에 따라 힘 주기
            enemyRigid.AddForce(knockbackDir * 5.0f, ForceMode.Impulse);
            enemyRigid.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
        }

        //사망 처리 요청
        if (curHP > 0)
            return;

        if (enemyHitCause != EnemyHitCause.Player)
        {
            StartCoroutine(EnemyDeadAfterTime());
            return;
        }

        Managers.Player.AcquireExp(1);
        if (Managers.Player.PlayerStat.Abilities.Contains(Ability.Vigilante))
            Managers.Money.Money += 50;

        StartCoroutine(EnemyDeadAfterTime());
    }

    /// <summary>
    /// 최초 접촉 후 1초마다 계속 trigger 상태인지 확인해서 피해를 입히는 로직
    /// </summary>
    IEnumerator StillTriggeredCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            Managers.Player.PlayerController.TakeDamage(1);
        }
    }


    IEnumerator EnemyDeadAfterTime()
    {
        yield return new WaitForSeconds(deadDelay);
        Destroy(gameObject);
    }

    #endregion

    #region 플레이어 추적
    private void FollowPlayer()
    {
        Vector3 delta = Managers.Player.PlayerController.transform.position - transform.position;
        if (delta.magnitude < 1f)
        {
            return;
        }
        float step = _moveSpeed * Time.deltaTime;
        Vector3 nextPos = Vector3.MoveTowards(_rb.position, Managers.Player.PlayerController.transform.position, step);
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