using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        _playerMover.SetDirection(input);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        _playerMover.SetRunning(context.ReadValueAsButton());
    }
}