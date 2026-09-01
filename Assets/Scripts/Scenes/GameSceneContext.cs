using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class rlacjfghks95_GameSceneContext : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("[rlacjfghks95_GameSceneContext] Game scene initialized.");
        StartCoroutine(GenerateNewDeliveryCard());
    }

    private IEnumerator GenerateNewDeliveryCard()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            Debug.Log("생성 시도");
            Define.HouseColor color = Managers.Deliver.GenerateDeliveryCard();
            // 이름 하나하나 찾아서 game object 를 변환하는 노가다를 해야하나?
            // house (특히 loop) 의 game object 리스트를 가져온다
            // 많은 house 들 중에서 랜덤으로 하나 선택
            // House 지붕에 color 적용
            // 참고: Define.HouseColors.Colors[color] 로 Color32, 컬러 코드 값 추출 가능
            // house 하나 랜덤으로 선택
            // 2번 줄에서 추출한 color 값으로 house 지붕 색상 변경
            // 해당 house 앞에 배달 완료 트리거가 될 npc 생성
            // npc 주변에 트리거 오브젝트 생성
        }
    }
}
