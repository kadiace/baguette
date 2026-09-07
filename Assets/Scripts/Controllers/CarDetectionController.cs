using UnityEngine;

public class CarDetectionController : MonoBehaviour
{
    private CarController _car;

    void Start()
    {
        _car = GetComponentInParent<CarController>();
    }

    void OnTriggerEnter(Collider other)
    {
        _car.OnTriggerEntered(CarTriggerType.Detection, other);
    }

    void OnTriggerExit(Collider other)
    {
        _car.OnTriggerExited(other);
    }
}