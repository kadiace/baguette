using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class rlacjfghks95_GameSceneContext : MonoBehaviour
{
    private UI_Deliveries _deliveries;

    private void Start()
    {
        Time.timeScale = 1f;

        _deliveries = Managers.UI.CreateUI<UI_Deliveries>();
        Debug.Log("[rlacjfghks95_GameSceneContext] Game scene initialized.");
    }

    private void Update()
    {

    }

    private IEnumerator GenerateNewDeliveryCard()
    {
        Debug.Log("생성 시작");

        yield return new WaitForSeconds(5f);

        Debug.Log("2초 후 실행");
    }
}
