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

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ShopPanel.SetActive(true);
        }
        else
        {
            ShopPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
