using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public Vector2 MoveDirection { get; private set; }
    public float CurrentSpeed => _rigidBody.velocity.magnitude;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, float speed)
    {
        MoveDirection = direction;
        _rigidBody.velocity = direction * speed;
    }

    public void Stop()
    {
        MoveDirection = Vector2.zero;
        _rigidBody.velocity = Vector2.zero;
    }
}
