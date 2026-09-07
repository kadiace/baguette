using UnityEngine;

public class CarCollisionController : MonoBehaviour
{
    private CarController _car;

    void Start()
    {
        _car = GetComponentInParent<CarController>();
    }

    void OnTriggerEnter(Collider other)
    {
        _car.OnTriggerEntered(CarTriggerType.Collision, other);
    }
}
