using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeliveryCard : UI_Base
{
    enum Texts
    {
        Address,
        Reward,
        Quantity,
        Time
    }

    private string _address;
    private int _reward;
    private int _quantity;
    private float _time;

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
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
        string address,
        float time,
        int quantity,
        int reward
    )
    {
        _address = address;
        _reward = reward;
        _quantity = quantity;
        _time = time;

        GetText((int)Texts.Address).GetComponent<TextMeshProUGUI>().text = _address;
        GetText((int)Texts.Reward).GetComponent<TextMeshProUGUI>().text = $"{_reward} €";
        GetText((int)Texts.Quantity).GetComponent<TextMeshProUGUI>().text = $"Baguette × {_quantity}";
        SetTime();
    }

    private void SetTime()
    {
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        GetText((int)Texts.Time).GetComponent<TextMeshProUGUI>().text = $"{minutes}:{seconds}";
    }
}
