using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliverManager
{
    private GameObject _deliveriesBackground;
    private int _maxLength = 3;
    private List<UI_DeliveryCard> _deliveryCards = new();

    public List<UI_DeliveryCard> DeliveryCards { get { return _deliveryCards; } }

    public void Init()
    {
        UI_Deliveries deliveries = Managers.UI.CreateUI<UI_Deliveries>(null, "Scenes");
        _deliveriesBackground = deliveries.transform.Find("Background").gameObject;
    }

    /// <summary>
    /// 새로운 배달을 추가
    /// </summary>
    public UI_DeliveryCard GenerateDeliveryCard()
    {
        if (_deliveryCards.Count >= _maxLength)
            return null;

        UI_DeliveryCard deliveryCard = Managers.UI.CreateUI<UI_DeliveryCard>(_deliveriesBackground.transform, "Components");

        Define.HouseColor color = (Define.HouseColor)UnityEngine.Random.Range(1,
            Enum.GetValues(typeof(Define.HouseColor)).Length);

        while (_deliveryCards.Exists(card => card.Color == color))
        {
            color = (Define.HouseColor)UnityEngine.Random.Range(1,
                Enum.GetValues(typeof(Define.HouseColor)).Length);
        }

        deliveryCard.SetCard(color, 1 * 60f, 5, 5);

        _deliveryCards.Add(deliveryCard);
        RefreshDeliveriesLayout();
        return deliveryCard;
    }

    public void CompleteDelivery(UI_DeliveryCard deliveryCard)
    {
        // deliveryCard reward 만큼 보상 획득, 바게트 차감

        // deliveryCard 리스트에서 제거 및 Destroy
        DestroyDeliveryCard(deliveryCard);
    }

    public void DestroyDeliveryCard(UI_DeliveryCard deliveryCard)
    {
        DeliveryCards.Remove(deliveryCard);
        RefreshDeliveriesLayout();
        Managers.Resource.Destroy(deliveryCard.transform.gameObject);
    }

    public bool IsHouseDuplicated(GameObject house)
    {
        return _deliveryCards.Exists(card => card.House == house);
    }

    private void RefreshDeliveriesLayout()
    {
        RectTransform container = _deliveriesBackground.GetComponent<RectTransform>();
        container.sizeDelta = new Vector2(container.sizeDelta.x, 60f + (80f * _deliveryCards.Count));
        for (int i = 0; i < _deliveryCards.Count; i++)
        {
            RectTransform rect = _deliveryCards[i].GetComponent<RectTransform>();

            // Scale 1, 1, 1
            rect.localScale = Vector3.one;

            // Anchor: Top Stretch
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);

            // Pivot: Left Top
            rect.pivot = new Vector2(0f, 1f);

            // Left 10 / Right 10
            rect.offsetMin = new Vector2(10f, rect.offsetMin.y);
            rect.offsetMax = new Vector2(-10f, rect.offsetMax.y);

            // Height 80
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 80f);

            // Pos Y
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                -60f - (80f * i)
            );
        }
    }
}
