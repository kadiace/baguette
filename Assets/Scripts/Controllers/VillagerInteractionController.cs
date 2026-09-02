using System.Runtime.CompilerServices;
using UnityEngine;

public class VillagerInteractionController : MonoBehaviour
{
    [Tooltip("PlayerController에서 WeaponHandler를 가져오는곳")]
    [SerializeField] private WeaponHandler weaponHandler;

    public WeaponHandler GetWeaponHandler()
    {
        return weaponHandler;
    }

    GameObject _roof;
    public GameObject Roof { set { _roof = value; } }

    Color _originHouseColor;

    public Color OriginHouseColor { set { _originHouseColor = value; } }

    UI_DeliveryCard _deliveryCard;

    public UI_DeliveryCard DeliveryCard { set { _deliveryCard = value; } }


    private void OnTriggerEnter(Collider other)
    {
        // 1. Collider other 가 player 인지 확인 (태그 비교), 아니면 return; 으로 함수를 종료
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // 2. collider를 player 스크립트로 연결 (어떻게 player 스크립트에 접근할 수 있을까?)
        PlayerController playerController = other.GetComponentInParent<PlayerController>();

        // 2.1. 플레이어컨트롤러를 통해 웨폰 핸들러를 가져오기
        WeaponHandler weaponHandler = playerController.weaponHandler;

        // 3. player 가 보유중인 바게트 개수를 확인 (제훈 > 바게트 개수 어디서 관리하세요?)
        int curBread = weaponHandler.curBread;

        // 4. 바게트 갯수가  deliveryCard.Quantity 보다 작으면 return; 으로 함수를 종료
        if (curBread < _deliveryCard.Quantity)
        {
            return;
        }
        // 5. 바게트 갯수를 즉시 차감 (deliveryCard.Quantity 만큼 차감)
        int newBread = curBread - _deliveryCard.Quantity;
        weaponHandler.curBread = newBread;

        // 6. 변경됐던 하우스 색상 기존 색상으로 원복
        gameObject.SetActive(false);

        Renderer[] renderers = _roof.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = _originHouseColor;
        }

        // 7. 배달 카드 완료 처리
        Managers.Deliver.CompleteDelivery(_deliveryCard);

        // 8. deliveryCard 에서 reward 만큼 보상 획득 (제훈 > 보상은 어디서 관리하세요?)

    }
}