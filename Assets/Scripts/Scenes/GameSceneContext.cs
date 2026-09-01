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
            Managers.Deliver.GenerateDeliveryCard();
        }
    }
}
