using System.Runtime.CompilerServices;
using UnityEngine;

public class PatissierInteractionController : MonoBehaviour
{
    [Tooltip("PlayerController에서 WeaponHandler를 가져오는곳")]
    [SerializeField] private WeaponHandler weaponHandler;

    public WeaponHandler GetWeaponHandler()
    {
        return weaponHandler;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 1. Collider other 가 player 인지 확인
        // 사용한 방법 : 태그를 비교해서 확인했습니다.
        if (!other.CompareTag("Player"))
        {
            return;
        }
        // 2. collider를 player 스크립트로 연결
        PlayerController playerController = other.GetComponentInParent<PlayerController>();

        // 3. 플레이어컨트롤러를 통해 웨폰 핸들러를 가져오기
        weaponHandler = playerController.weaponHandler;

        // 4. 최대빵을 불러와요
        weaponHandler.SupplyBread();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
