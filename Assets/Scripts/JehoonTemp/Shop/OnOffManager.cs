using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OnOffManager : MonoBehaviour
{
    [Tooltip("상점 패널")]
    [SerializeField] GameObject ShopPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
            ShopPanel.GetComponent<ShopManager>().SetMoneyValue();

            ShopPanel.GetComponent<ShopManager>().ButtonInitiate();

            Managers.Game.Paused = true;
            ShopPanel.SetActive(true);
        }
        else
        {
            Managers.Game.Paused = false;
            ShopPanel.SetActive(false);
        }
    }
}
