using TMPro;
using UnityEngine;

public class MoneyManager
{
    private float _money;

    public float Money
    {
        get { return _money; }
        set
        {
            _money = value;
            if (_moneyUI == null)
                return;
            _moneyUI.text = "€ " + _money.ToString("F2");
        }
    }
    private int _drinkCount;
    private int _butterCount;

    private TextMeshProUGUI _moneyUI;

    public TextMeshProUGUI MoneyUI { set { _moneyUI = value; _moneyUI.text = "€ " + _money.ToString("F2"); } }
    public int DrinkCount { get { return _drinkCount; } set { _drinkCount = value; } }
    public int ButterCount { get { return _butterCount; } set { _butterCount = value; } }

    public void Init()
    {
        _money = 5700f;
        _drinkCount = 5;
        _butterCount = 5;
    }
}
