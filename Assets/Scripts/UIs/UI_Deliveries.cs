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
}
