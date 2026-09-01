using System.Collections.Generic;
using UnityEngine;

public class CarController : Poolable
{
    [SerializeField]
    private float _speed = 30f;


    [SerializeField] private float _targetScale = 10f;

    private int _step = 1;
    private List<Vector3> _path;

    public List<Vector3> Path
    {
        get { return _path; }
        set
        {
            if (value == null || value.Count == 0)
                return;
            _path = value;
            transform.position = _path[0];
        }
    }


    private void Start()
    {

    }
    private void Update()
    {
        if (_path == null)
            return;


        if (_step == _path.Count)
        {
            Destroy(gameObject);
        }

        Vector3 diff = _path[_step] - transform.position;
        if (diff.magnitude < 0.1f)
        {
            transform.position = _path[_step];
            _step += 1;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            _path[_step],
            _speed * Time.deltaTime
        );
        transform.rotation = Quaternion.LookRotation(diff.normalized, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 closestPoint = other.ClosestPoint(transform.position);
        Vector3 forceDirection = closestPoint - transform.position;
        forceDirection += Vector3.up;

        forceDirection.Normalize();
        Debug.Log($"{forceDirection}");

        other.attachedRigidbody.AddForce(forceDirection * _targetScale, ForceMode.VelocityChange);
    }
}