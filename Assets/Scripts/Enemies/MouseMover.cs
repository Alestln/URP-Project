using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class MouseMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;

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

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    public void Stop()
    {
        _moveDirection = Vector2.zero;
        // _rigidbody.velocity = Vector2.zero;
    }

    private void Move()
    {
        _rigidbody.velocity = _moveDirection * _moveSpeed;
    }
}
