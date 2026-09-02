using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI moneyUI = gameObject.GetorAddComponent<TextMeshProUGUI>();
        Managers.Money.MoneyUI = moneyUI;
    }
}
