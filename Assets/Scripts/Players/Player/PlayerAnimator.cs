using UnityEngine;

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