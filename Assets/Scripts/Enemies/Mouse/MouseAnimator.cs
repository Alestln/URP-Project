using UnityEngine;

public static class MouseAnimationParams
{
    public const string DirectionX = "DirectionX";
    public const string DirectionY = "DirectionY";
    public const string Speed = "Speed";
}

[RequireComponent(typeof(Animator))]
public class MouseAnimator : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("Reference to the MouseMover component to get movement direction and speed.")]
    [SerializeField] private MouseMover _mouseMover;

    private Animator _animator;

    private int SpeedHash = Animator.StringToHash(MouseAnimationParams.Speed);
    private int DirectionXHash = Animator.StringToHash(MouseAnimationParams.DirectionX);
    private int DirectionYHash = Animator.StringToHash(MouseAnimationParams.DirectionY);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimationParameters();
    }

    private void UpdateAnimationParameters()
    {
        float speed = _mouseMover.CurrentSpeed;

        _animator.SetFloat(SpeedHash, speed);
    }

    public void SetDirection(Vector2 direction)
    {
        _animator.SetFloat(DirectionXHash, direction.x);
        _animator.SetFloat(DirectionYHash, direction.y);
    }
}
