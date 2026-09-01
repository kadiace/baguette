using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Deliveries : UI_Base
{
    private List<UI_DeliveryCard> _deliveryCards;

    public List<UI_DeliveryCard> DeliveryCards { get { return _deliveryCards; } }

    public int MaxLength = 3;

    public override void Init()
    {
    }

    private void Update()
    {

    }

    public void GenerateDeliveryCard()
    {
        UI_DeliveryCard deliveryCard = Managers.UI.CreateUI<UI_DeliveryCard>(gameObject.transform);
    }

    public void EnterHome(Define.Address address)
    {
        for (int i = 0; i < MaxLength; i++)
        {
            UI_DeliveryCard deliveryCards = _deliveryCards[i];

        }
    }

    public void DestoryDeliveryCard(UI_DeliveryCard deliveryCard)
    {
        DeliveryCards.Remove(deliveryCard);
        Destroy(deliveryCard);
    }
}
