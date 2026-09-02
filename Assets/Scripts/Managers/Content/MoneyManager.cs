using UnityEngine;

public class MoneyManager
{
    private float _money;
    private int _drinkCount;
    private int _butterCount;

    public float Money { get { return _money; } set { _money = value; } }
    public int DrinkCount { get { return _drinkCount; } set { _drinkCount = value; } }
    public int ButterCount { get { return _butterCount; } set { _butterCount = value; } }

    public void Init()
    {
        _money = 5700f;
        _drinkCount = 5;
        _butterCount = 5;
    }

}