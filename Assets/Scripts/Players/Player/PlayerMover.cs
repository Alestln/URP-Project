using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private float _currentSpeed;

    public Vector2 MoveDirection { get; private set; }
    public float CurrentSpeed => _rigidBody.velocity.magnitude;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rigidBody.velocity = MoveDirection * _currentSpeed;
    }

    public void Move(Vector2 direction, float speed)
    {
        MoveDirection = direction;
        _currentSpeed = speed;
    }

    public void Stop()
    {
        MoveDirection = Vector2.zero;
        _rigidBody.velocity = Vector2.zero;
    }
}
