using UnityEngine;

public class ShopkeeperController : MonoBehaviour
{
    [Tooltip("PlayerController에서 WeaponHandler를 가져오는곳")]
    [SerializeField] private WeaponHandler weaponHandler;

    public WeaponHandler GetWeaponHandler()
    {
        return weaponHandler;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
