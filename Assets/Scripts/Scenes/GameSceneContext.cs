using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class rlacjfghks95_GameSceneContext : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1f;

        Debug.Log("[rlacjfghks95_GameSceneContext] Game scene initialized.");
    }

    private void Update()
    {
        StartCoroutine(GenerateNewDeliveryCard());
    }

    private IEnumerator GenerateNewDeliveryCard()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            Debug.Log("생성 시도");
            Managers.Deliver.GenerateDeliveryCard();
        }
    }
}
