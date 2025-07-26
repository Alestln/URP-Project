using UnityEngine;
public static class PLayerAnimationNames
{
    public const string DirectionX = "DirectionX";
    public const string DirectionY = "DirectionY";
    public const string Speed = "Speed";
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
        _animator.SetFloat(PLayerAnimationNames.DirectionX, _playerMover.MoveDirection.x);
        _animator.SetFloat(PLayerAnimationNames.DirectionY, _playerMover.MoveDirection.y);
        _animator.SetFloat(PLayerAnimationNames.Speed, _playerMover.CurrentSpeed);
    }
}