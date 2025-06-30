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

    /*public float CurrentSpeed => _rigidBody.velocity.magnitude;
    public Vector2 MoveDirection => _direction;
    public bool IsRunning => _isRunning;*/

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
        _direction = direction;
    }

    public void SetRunning(bool isRunning)
    {
        _isRunning = isRunning;
    }

    private void Move()
    {
        float targetSpeed = _isRunning ? _runSpeed : _walkSpeed;
        _rigidBody.velocity = _direction * targetSpeed;
    }
}
