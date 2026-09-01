using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliverManager
{
    private UI_Deliveries _deliveries;
    private List<UI_DeliveryCard> _deliveryCards;

    public List<UI_DeliveryCard> DeliveryCards { get { return _deliveryCards; } }

    public int MaxLength = 3;

    public void GenerateDeliveryCard()
    {
        Managers.UI.CreateUI<UI_DeliveryCard>();
    }

    public void EnterHome(Define.HouseColor color)
    {
        for (int i = 0; i < MaxLength; i++)
        {
            UI_DeliveryCard deliveryCards = _deliveryCards[i];

        }
    }

    public void DestoryDeliveryCard(UI_DeliveryCard deliveryCard)
    {
        DeliveryCards.Remove(deliveryCard);
        Managers.Resource.Destory(deliveryCard.transform.gameObject);
    }
}
