using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryPair
{
    public UI_DeliveryCard Card;
    public GameObject House;
    public VillagerInteractionController Villager;
    public Color OriginHouseColor;
}

public class DeliverManager
{
    private GameObject _deliveriesBackground;
    private int _maxLength = 3;
    private List<DeliveryPair> _deliveries = new();

    public int MaxLength { get { return _maxLength; } set { _maxLength = value; } }
    public List<DeliveryPair> Deliveries { get { return _deliveries; } }

    public void Init()
    {
        UI_Deliveries deliveries = Managers.UI.CreateUI<UI_Deliveries>(null, "Scenes");
        _deliveriesBackground = deliveries.transform.Find("Background").gameObject;
    }

    public bool IsFull()
    {
        return _deliveries.Count >= _maxLength;
    }

    public void GenerateDeliveryCard(GameObject house, int maxBread)
    {
        // Card
        UI_DeliveryCard deliveryCard = Managers.UI.CreateUI<UI_DeliveryCard>(_deliveriesBackground.transform, "Components");

        HouseColor color = (HouseColor)UnityEngine.Random.Range(1,
            Enum.GetValues(typeof(HouseColor)).Length);

        while (_deliveries.Select(x => x.Card)
            .ToList().Exists(card => card.Color == color))
        {
            color = (HouseColor)UnityEngine.Random.Range(1,
                Enum.GetValues(typeof(HouseColor)).Length);
        }

        // Pick quantity
        int upgradeLevel = (maxBread - 5) / 2;
        int minQuantity = Mathf.RoundToInt(maxBread * 0.2f); // 1/5
        int maxQuantity = Mathf.RoundToInt(maxBread * 0.4f); // 2/5
        int requiredBread = UnityEngine.Random.Range(minQuantity, maxQuantity + 1);

        int reward = 180
           + upgradeLevel * 2
           + requiredBread * 20;

        deliveryCard.SetCard(color, 1 * 60f, requiredBread, reward);

        // House
        Transform selectedRoof = house.transform.Find("Roof");
        Transform selectedVillager = house.transform.Find("Villager");

        Renderer[] renderers = selectedRoof.GetComponentsInChildren<Renderer>();
        Color originHouseColor = renderers[0].material.color;
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = ColorCatalog.Info[deliveryCard.Color];
        }

        GameObject trigger = selectedVillager.Find("Trigger").gameObject;
        trigger.SetActive(true);

        VillagerInteractionController villagerInteractionController = trigger.GetorAddComponent<VillagerInteractionController>();
        villagerInteractionController.Roof = selectedRoof.gameObject;

        // Deliver Manager
        _deliveries.Add(new DeliveryPair
        {
            Card = deliveryCard,
            House = house,
            Villager = villagerInteractionController,
            OriginHouseColor = originHouseColor
        });
        RefreshDeliveriesLayout();
    }

    public void CompleteDelivery(PlayerController player, VillagerInteractionController villager)
    {
        // Reduce bread
        WeaponHandler weaponHandler = player.weaponHandler;
        DeliveryPair pair = _deliveries.Find(delivery => delivery.Villager == villager);
        UI_DeliveryCard deliveryCard = pair.Card;
        Color originHouseColor = pair.OriginHouseColor;

        if (Managers.Player.PlayerStat.Bread < deliveryCard.Quantity)
            return;

        Managers.Player.PlayerStat.Bread -= deliveryCard.Quantity;

        // Increase Health
        player.IncreaseHealth();

        // deliveryCard 리스트에서 제거 및 Destroy
        DestroyDelivery(pair);

        // Earn Money & Get Exp
        Managers.Money.Money = Managers.Money.Money + deliveryCard.Reward;
        Managers.Player.AcquireExp(5);
    }

    public void DestroyDelivery(UI_DeliveryCard deliveryCard)
    {
        DeliveryPair pair = _deliveries.Find(delivery => delivery.Card == deliveryCard);
        DestroyDelivery(pair);
    }

    public void DestroyDelivery(DeliveryPair pair)
    {
        UI_DeliveryCard deliveryCard = pair.Card;
        GameObject house = pair.House;
        VillagerInteractionController villager = pair.Villager;
        Color originHouseColor = pair.OriginHouseColor;

        // Restore roof color
        Transform roof = house.transform.Find("Roof");
        Renderer[] renderers = roof.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = originHouseColor;
        }

        // Inactive villager trigger
        villager.gameObject.SetActive(false);

        // Destroy UI Card
        Managers.Resource.Destroy(deliveryCard.transform.gameObject);

        _deliveries.Remove(pair);
        RefreshDeliveriesLayout();
    }

    public bool IsHouseDuplicated(GameObject house)
    {
        return _deliveries.Exists(delivery => delivery.House == house);
    }

    private void RefreshDeliveriesLayout()
    {
        RectTransform container = _deliveriesBackground.GetComponent<RectTransform>();
        container.sizeDelta = new Vector2(container.sizeDelta.x, 60f + (60f * _deliveries.Count));
        for (int i = 0; i < _deliveries.Count; i++)
        {
            RectTransform rect = _deliveries[i].Card.GetComponent<RectTransform>();

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

            // Height 60
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 60f);

            // Pos Y
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                -60f - (60f * i)
            );
        }
    }

    public void Clear()
    {
        while (_deliveries.Count > 0)
            DestroyDelivery(_deliveries[_deliveries.Count - 1]);
    }
}
