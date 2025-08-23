using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseMover : MonoBehaviour
{
    private float _moveSpeed = 0f; // —корость движени€ мыши, инициализируетс€ через SetSpeed

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;

    public float CurrentSpeed => _rigidbody.velocity.magnitude;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    private void Move()
    {
        _rigidbody.velocity = _moveDirection * _moveSpeed;
    }
}
