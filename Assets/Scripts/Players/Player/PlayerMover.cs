using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidBody;

    private Vector2 _direction;

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

    private void Move()
    {
        _rigidBody.velocity = _direction * _speed;
    }
}
