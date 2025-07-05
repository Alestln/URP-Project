using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;

    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    private bool _isRunning;
    private bool _isMoving;

    public Vector2 MoveDirection => _direction;
    public float CurrentSpeed => _rigidBody.velocity.magnitude;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            _isMoving = false;
        }
        else
        {
            _direction = direction;
            _isMoving = true;
        }
    }

    public void SetRunning(bool isRunning)
    {
        _isRunning = isRunning;
    }

    private void Move()
    {
        if (!_isMoving)
        {
            _rigidBody.velocity = Vector2.zero;
            return;
        }

        float targetSpeed = _isRunning ? _runSpeed : _walkSpeed;
        _rigidBody.velocity = _direction * targetSpeed;
    }
}
