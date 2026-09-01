using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeliveryCard : UI_Base
{
    enum Images
    {
        House,
    }

    enum Texts
    {
        Reward,
        Quantity,
        Time
    }

    private GameObject _house;
    private Define.HouseColor _color;
    private int _reward;
    private int _quantity;
    private float _time;

    public GameObject House { get { return _house; } set { _house = value; } }
    public Define.HouseColor Color { get { return _color; } }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    private void Update()
    {
        if (_time <= 0f)
            return;

        _time -= Time.deltaTime;
        _time = Mathf.Max(_time, 0f);

        if (_time <= 0f)
        {
            Managers.Deliver.DestoryDeliveryCard(this);
            return;
        }

        SetTime();
    }

    public void SetCard(
        Define.HouseColor color,
        float time,
        int quantity,
        int reward
    )
    {
        _color = color;
        _reward = reward;
        _quantity = quantity;
        _time = time;

        GetText((int)Texts.Reward).GetComponent<TextMeshProUGUI>().text = $"{_reward} €";
        GetText((int)Texts.Quantity).GetComponent<TextMeshProUGUI>().text = $"Baguette × {_quantity}";
        GetImage((int)Images.House).GetComponent<Image>().color = Define.HouseColors.Colors[_color];
        SetTime();
    }

    private void SetTime()
    {
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        GetText((int)Texts.Time).GetComponent<TextMeshProUGUI>().text = $"{minutes}:{seconds}";
    }
}
