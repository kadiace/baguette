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
            GameObject houses = GameObject.Find("Houses");
            int count = houses.transform.childCount;

            List<GameObject> houseList = new List<GameObject>();

            for (int i = 0; i < count; i++)
            {
                Transform child = houses.transform.GetChild(i);
                houseList.Add(child.gameObject);
            }
            
            // 많은 house 들 중에서 랜덤으로 하나 선택
            GameObject selectedHouse = houseList[Random.Range(0, houseList.Count)];

            // 선택된 house 의 지붕, 주민을 선택
            Transform selectedRoof = selectedHouse.transform.Find("Roof");
            Transform selectedVillager = selectedHouse.transform.Find("Villager");

            // House 지붕에 color 적용
            // 참고: Define.HouseColors.Colors[color] 로 Color32, 컬러 코드 값 추출 가능
            
            Renderer renderer = selectedRoof.GetComponent<Renderer>();
            renderer.material.color = Define.HouseColors.Colors[color];

            // npc 주변에 트리거 오브젝트 활성화
            GameObject trigger = selectedVillager.Find("Trigger_Range").gameObject;
            trigger.SetActive(true);
        }
    }
}
