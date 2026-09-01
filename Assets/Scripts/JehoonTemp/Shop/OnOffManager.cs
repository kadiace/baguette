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
        if(!ShopPanel.activeSelf)
        {
            ShopPanel.SetActive(true);
        }
        else
        {
            ShopPanel.SetActive(false);
        }
    }
}
