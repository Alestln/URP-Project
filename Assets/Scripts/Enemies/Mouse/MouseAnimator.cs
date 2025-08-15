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
    private Animator _animator;

    private int DirectionXHash = Animator.StringToHash(MouseAnimationParams.DirectionX);
    private int DirectionYHash = Animator.StringToHash(MouseAnimationParams.DirectionY);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        _animator.SetFloat(DirectionXHash, direction.x);
        _animator.SetFloat(DirectionYHash, direction.y);
    }
}
