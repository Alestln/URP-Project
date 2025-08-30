using UnityEngine;
public static class PlayerAnimationNames
{
    public const string DirectionX = "DirectionX";
    public const string DirectionY = "DirectionY";
    public const string Speed = "Speed";
    public const string Attack = "Attack";
}

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private PlayerMover _playerMover;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat(PlayerAnimationNames.DirectionX, _playerMover.MoveDirection.x);
        _animator.SetFloat(PlayerAnimationNames.DirectionY, _playerMover.MoveDirection.y);
        _animator.SetFloat(PlayerAnimationNames.Speed, _playerMover.CurrentSpeed);
    }
}