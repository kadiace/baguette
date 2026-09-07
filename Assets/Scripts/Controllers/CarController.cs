using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : Poolable
{
    [SerializeField]
    private float _speed = 120f;


    [SerializeField] private float _collisionForce = 5f;
    [SerializeField] private float _floatForce = 15f;
    [SerializeField] private InputAction _moveInput;
    [SerializeField] private InputAction _interactionInput;

    private readonly HashSet<GameObject> _collidedObjects = new();

    private int _step = 1;
    private List<Vector3> _path;
    private bool _isRide;
    private PlayerController _player;
    private Rigidbody _rb;
    private Vector3 _playerLocalPos;
    private Quaternion _playerLocalRot;

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

    void Update()
    {
        if (!_isRide)
            FollowRoute();

        if (_interactionInput.triggered)
            Dismount();
    }

    void FixedUpdate()
    {
        if (!_isRide)
            return;

        DriveCar();

        // [-70, 70] 내부에 있도록 보정
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -70f, 70f);
        pos.z = Mathf.Clamp(pos.z, -70f, 70f);

        transform.position = pos;
    }

    private void DriveCar()
    {
        _player.CamController.CameraAim(false);

        Vector2 movePos = _moveInput.ReadValue<Vector2>();

        if (movePos.sqrMagnitude < 0.001f)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            return;
        }

        if (movePos.sqrMagnitude < 0.001f)
            return;

        Vector3 camForward = _player.CamController.transform.forward;
        Vector3 camRight = _player.CamController.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = (camForward * movePos.y) + (camRight * movePos.x);

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
            return;

        _rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);
        _rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
    }

    private void FollowRoute()
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

    public void Ride(PlayerController player)
    {
        player.transform.SetParent(transform);
        _playerLocalPos = player.transform.localPosition;
        _playerLocalRot = Quaternion.identity;
        player.transform.localPosition = Vector3.zero;
        _player = player;
        player.gameObject.SetActive(false);

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0f, -0.5f, 0.4f);
        box.size = new Vector3(3f, 2.93f, 4.5f);

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        _rb = rb;
        _rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _isRide = true;
        _moveInput.Enable();
        StartCoroutine(EnableInputAfterDelay(0.5f));
    }

    private void Dismount()
    {
        _player.transform.localPosition = _playerLocalPos;
        _player.transform.localRotation = _playerLocalRot;
        _player.transform.SetParent(null);
        _player.gameObject.SetActive(true);
        _player = null;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
            Destroy(box);

        Destroy(_rb);

        _isRide = false;
        _moveInput.Disable();
        _interactionInput.Disable();
    }

    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _interactionInput.Enable();
    }

    public void OnTriggerEntered(CarTriggerType carTriggerType, Collider other)
    {
        switch (carTriggerType)
        {
            case CarTriggerType.Collision:
                HandleCollisionTrigger(other);
                break;
            case CarTriggerType.Detection:
                HandleDetectionTrigger(other);
                break;
        }
    }

    public void OnTriggerExited(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().Cars.Remove(this);
    }

    private void HandleCollisionTrigger(Collider other)
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
        _collidedObjects.Add(go);
    }

    private void HandleDetectionTrigger(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        other.GetComponent<PlayerController>().Cars.Add(this);
    }
}
