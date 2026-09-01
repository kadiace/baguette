using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class TrafficPath
{
    public List<Vector3> Waypoints;
}

public class GameSceneContext : MonoBehaviour
{
    [SerializeField]
    private List<TrafficPath> _paths;

    private void Start()
    {
        Debug.Log("[GameSceneContext] Game scene initialized.");
        StartCoroutine(RepeatAction(3f, GenerateNewDeliveryCard));
        StartCoroutine(RepeatAction(1f, SpawnTraffic));
        StartCoroutine(RepeatAction(0.5f, SpawnPickPocket));
    }

    private IEnumerator RepeatAction(float period, Action function)
    {
        while (true)
        {
            yield return new WaitForSeconds(period);
            function();
        }
    }

    private void GenerateNewDeliveryCard()
    {
        UI_DeliveryCard deliveryCard = Managers.Deliver.GenerateDeliveryCard();
        if (deliveryCard != null)
        {
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
            GameObject selectedHouse = houseList[UnityEngine.Random.Range(0, houseList.Count)];
            while (Managers.Deliver.IsHouseDuplicated(selectedHouse))
            {
                selectedHouse = houseList[UnityEngine.Random.Range(0, houseList.Count)];
            }

            // 선택된 house 의 지붕, 주민을 선택
            Transform selectedRoof = selectedHouse.transform.Find("Roof");
            Transform selectedVillager = selectedHouse.transform.Find("Villager");

            // House 지붕에 color 적용
            // 참고: Define.HouseColors.Colors[color] 로 Color32, 컬러 코드 값 추출 가능
            Renderer[] renderers = selectedRoof.GetComponentsInChildren<Renderer>();
            Color originRoofColor = renderers[0].material.color;
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = Define.HouseColors.Colors[deliveryCard.Color];
            }

            // npc 주변에 트리거 오브젝트 활성화
            GameObject trigger = selectedVillager.Find("Trigger").gameObject;
            trigger.SetActive(true);

            // trigger enter 후 처리를 위해 정보 저장
            VillagerInteractionController villagerInteractionController = trigger.GetorAddComponent<VillagerInteractionController>();
            villagerInteractionController.OriginHouseColor = originRoofColor;
            villagerInteractionController.DeliveryCard = deliveryCard;
            villagerInteractionController.Roof = selectedRoof.gameObject;
        }
    }

    private void SpawnTraffic()
    {
        GameObject go = Managers.Resource.Instantiate("Car");
        CarController car = go.GetorAddComponent<CarController>();
        car.Path = _paths[UnityEngine.Random.Range(0, _paths.Count)].Waypoints;
    }

    private void SpawnPickPocket()
    {
        GameObject go = Managers.Resource.Instantiate("EnemyTemp");
        EnemyController pickPocket = go.GetorAddComponent<EnemyController>();
        go.transform.position = GetRandomEdgePosition();

    }

    private Vector3 GetRandomEdgePosition()
    {
        float r = UnityEngine.Random.Range(-75f, 75f);

        return UnityEngine.Random.Range(0, 4) switch
        {
            0 => new Vector3(75f, 1f, r),
            1 => new Vector3(-75f, 1f, r),
            2 => new Vector3(r, 1f, 75f),
            _ => new Vector3(r, 1f, -75f),
        };
    }
}
