using TMPro;

public class MoneyManager
{
    private float _money;
    private OverHeadIconHandler _overHeadIconHandler;

    public float Money
    {
        get { return _money; }
        set
        {
            if (value > _money)
                OverHeadIconHandler.StartShowEuro();
            _money = value;
            if (_moneyUI == null)
                return;
            _moneyUI.text = "€ " + _money.ToString("F2");
        }
    }
    public OverHeadIconHandler OverHeadIconHandler { get { return _overHeadIconHandler; } set { _overHeadIconHandler = value; } }

    private TextMeshProUGUI _moneyUI;

    public TextMeshProUGUI MoneyUI { set { _moneyUI = value; _moneyUI.text = "€ " + _money.ToString("F2"); } }

    public void Init()
    {
        _money = 10000f;
    }

    public void Clear()
    {
        _money = 10000f;
    }
}
