using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : Poolable
{
    [SerializeField]
    private float _speed = 120f;


    [SerializeField] private float _collisionForce = 5f;
    [SerializeField] private float _floatForce = 15f;

    private readonly HashSet<GameObject> _collidedObjects = new();

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

    private void Update()
    {
        if (_path == null)
            return;


        if (_step == _path.Count)
        {
            Managers.Resource.Destroy(gameObject);
            _step = 1;
            _collidedObjects.Clear();
            return;
        }

        Vector3 delta = _path[_step] - transform.position;
        if (delta.magnitude < 0.01f)
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
        if (delta.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Player") || other.CompareTag("Enemy")))
            return;

        GameObject go = other.gameObject;
        if (_collidedObjects.Contains(go))
            return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
            return;

        Vector3 delta = rb.transform.position - transform.position;
        delta.y = 0;
        Vector3 carVelocity = (_path[_step] - transform.position).normalized * _speed;
        Vector3 forceDir = delta.normalized;

        forceDir.Normalize();
        rb.AddForce(carVelocity * 0.1f + forceDir * _collisionForce + Vector3.up * _floatForce, ForceMode.Impulse);

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamage(1);
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.EnemyHit(3, EnemyHitCause.Car);
        }
        //플레이어 피격

        _collidedObjects.Add(go);
    }
}
