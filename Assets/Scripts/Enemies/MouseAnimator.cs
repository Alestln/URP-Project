using UnityEngine;

public static class EnemyAnimationParams
{
    public const string Speed = "Speed";
    public const string DirectionX = "DirectionX";
    public const string DirectionY = "DirectionY";
}

[RequireComponent(typeof(Animator))]
public class MouseAnimator : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("—сылка на компонент движени€ врага дл€ получени€ данных о скорости.")]
    [SerializeField] private MouseMover _enemyMover;

    private Animator _animator;

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
        float speed = _enemyMover.CurrentSpeed;

        _animator.SetFloat(EnemyAnimationParams.Speed, speed);
    }

    public void SetDirection(Vector2 direction)
    {
        _animator.SetFloat(EnemyAnimationParams.DirectionX, direction.x);
        _animator.SetFloat(EnemyAnimationParams.DirectionY, direction.y);
    }
}
