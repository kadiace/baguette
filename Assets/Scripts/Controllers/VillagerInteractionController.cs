using UnityEngine;

public class VillagerInteractionController : MonoBehaviour
{
    GameObject _roof;
    public GameObject Roof { set { _roof = value; } }

    Color _originHouseColor;

    public Color OriginHouseColor { set { _originHouseColor = value; } }

    UI_DeliveryCard _deliveryCard;

    public UI_DeliveryCard DeliveryCard { set { _deliveryCard = value; } }


    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

        Renderer[] renderers = _roof.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = _originHouseColor;
        }
        Managers.Deliver.CompleteDelivery(_deliveryCard);
    }
}