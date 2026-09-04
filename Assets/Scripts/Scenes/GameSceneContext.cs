using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficPath
{
    public List<Vector3> Waypoints;
}

public class GameSceneContext : BaseScene
{
    [SerializeField]
    private List<TrafficPath> _paths;

    private void Start()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(RepeatAction(10f, GenerateNewDeliveryCard));
        StartCoroutine(RepeatAction(5f, SpawnTraffic));
        StartCoroutine(RepeatAction(2f, SpawnPickPocket));

        Managers.UI.CreateUI<UI_InGame>(null, "Scenes");
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
        if (Managers.Deliver.IsFull())
            return;

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

        Managers.Deliver.GenerateDeliveryCard(selectedHouse, Managers.Player.PlayerStat.MaxBread);
    }

    private void SpawnTraffic()
    {
        if (Managers.Pool.GetStackSize("Car") >= 10)
            return;
        GameObject go = Managers.Resource.Instantiate("NPCs/Car");
        CarController car = go.GetorAddComponent<CarController>();
        car.Path = _paths[UnityEngine.Random.Range(0, _paths.Count)].Waypoints;
    }

    private void SpawnPickPocket()
    {
        int upgradeLevel = (Managers.Player.PlayerStat.MaxBread - 5) / 2;
        if (Managers.Pool.GetStackSize("Pickpocket") >= 6 + 1 * upgradeLevel)
            return;
        GameObject go = Managers.Resource.Instantiate("NPCs/Pickpocket");
        EnemyController pickPocket = go.GetorAddComponent<EnemyController>();
        go.transform.position = GetRandomEdgePosition();
    }

    private Vector3 GetRandomEdgePosition()
    {
        float r = UnityEngine.Random.Range(-70f, 70f);

        return UnityEngine.Random.Range(0, 4) switch
        {
            0 => new Vector3(70f, 1f, r),
            1 => new Vector3(-70f, 1f, r),
            2 => new Vector3(r, 1f, 70f),
            _ => new Vector3(r, 1f, -70f),
        };
    }

    public override void Clear()
    {

    }
}
