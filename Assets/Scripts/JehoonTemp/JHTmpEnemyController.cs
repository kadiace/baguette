using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class JHTmpEnemyController : MonoBehaviour
{
    [Tooltip("플레이어")]
    [SerializeField] private JHTmpPlayerController player;
    [Tooltip("적 이동 속도")]
    [SerializeField] private float enemySpeed = 3f;

    private Rigidbody enemyRigid;

    public UnityEvent<int> onPlayerDamaged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerTemp").GetComponent<JHTmpPlayerController>();
        enemyRigid = GetComponent<Rigidbody>();
        onPlayerDamaged = new UnityEvent<int>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
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
            StopCoroutine(StillTriggeredCoroutine());
            player.TakeDamage(1);
            Debug.Log("플레이어가 적에게 피해를 입었습니다. 현재 체력: " + player.GetCurrentHealth());
            StartCoroutine(StillTriggeredCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerTemp")
        {
            StopCoroutine(StillTriggeredCoroutine());
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
        float step = enemySpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }
    #endregion
}