using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerInteractionController : MonoBehaviour
{
    [Tooltip("PlayerController에서 WeaponHandler를 가져오는곳")]
    [SerializeField] private WeaponHandler weaponHandler;

    public WeaponHandler GetWeaponHandler()
    {
        return weaponHandler;
    }

    GameObject _roof;
    public GameObject Roof { get { return _roof; } set { _roof = value; } }


    private void OnTriggerEnter(Collider other)
    {
        bool player = other.CompareTag("Player");
        bool throwDelivery = other.CompareTag("Bread") && Managers.Player.PlayerStat.Abilities.Contains(Ability.ThrowDelivery);
        if (!player && !throwDelivery)
            return;

        Managers.Deliver.CompleteDelivery(this, other.tag);

        if (throwDelivery)
            Destroy(other);
    }
}
