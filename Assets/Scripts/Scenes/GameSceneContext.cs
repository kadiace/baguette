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
    private BreadCounter _breadCounter;

    [SerializeField]
    private List<TrafficPath> _paths;

    private List<Vector3> _spawnPositions = new()
    {
        new Vector3(11.0725107f,1f,-34.3516426f),
        new Vector3(11.0725107f,1f,-34.3516426f),
        new Vector3(-14f,1f,-53.7999992f),
        new Vector3(-21.6000004f,1f,-13f),
        new Vector3(-31.7000008f,1f,-4.69999981f),
        new Vector3(33f,1f,-24.6000004f),
        new Vector3(6.9000001f,1f,10.1999998f),
        new Vector3(40.7999992f,1f,-1.79999995f),
        new Vector3(28.7999992f,1f,55.4000015f),
        new Vector3(35.2000008f,1f,-73.4000015f),
        new Vector3(-46.7999992f,1f,-49.0999985f),
        new Vector3(-43f,1f,-33.5f),
        new Vector3(-61.5999985f,1f,4.80000019f),
        new Vector3(59.2000008f,1f,-14.8000002f),
        new Vector3(-53f,1f,53.7000008f),
    };

    private void Start()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SpawnInitPickPocket();

        StartCoroutine(RepeatAction(10f, GenerateNewDeliveryCard));
        StartCoroutine(RepeatAction(5f, SpawnTraffic));
        StartCoroutine(RepeatAction(2f, SpawnPickPocket));
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

        Managers.Deliver.GenerateDeliveryCard(selectedHouse, _breadCounter.GetMaxBread());
    }

    private void SpawnTraffic()
    {
        if (Managers.Pool.GetStackSize("Car") >= 10)
            return;
        GameObject go = Managers.Resource.Instantiate("Car");
        CarController car = go.GetorAddComponent<CarController>();
        car.Path = _paths[UnityEngine.Random.Range(0, _paths.Count)].Waypoints;
    }

    private void SpawnInitPickPocket()
    {
        foreach (Vector3 spawnPosition in _spawnPositions)
        {
            GameObject go = Managers.Resource.Instantiate("Pickpocket");
            go.transform.position = spawnPosition;
            EnemyController pickPocket = go.GetorAddComponent<EnemyController>();
        }
    }

    private void SpawnPickPocket()
    {
        if (Managers.Pool.GetStackSize("Pickpocket") >= 15)
            return;
        GameObject go = Managers.Resource.Instantiate("Pickpocket");
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
}
