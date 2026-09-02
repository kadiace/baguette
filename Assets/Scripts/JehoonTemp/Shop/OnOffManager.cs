using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OnOffManager : MonoBehaviour
{
    [Tooltip("상점 패널")]
    [SerializeField] GameObject ShopPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Tooltip("파워업 소모품 관리 스크립트")]
    [SerializeField] PowerUpManager powerUpManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StateChange()
    {
        if (!ShopPanel.activeSelf)
        {
            ShopPanel.GetComponent<ShopManager>().SetDrinkValue();
            ShopPanel.GetComponent<ShopManager>().SetButterValue();
            ShopPanel.SetActive(true);
        }
        else
        {
            powerUpManager.SetDrinkValue();
            powerUpManager.SetButterValue();
            ShopPanel.SetActive(false);
        }
    }
}
