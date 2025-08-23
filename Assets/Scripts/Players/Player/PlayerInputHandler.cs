using Assets.Scripts.Players.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    private PlayerInputData _inputData;

    private void Update()
    {
        _playerController.SetInput(_inputData);

        _inputData.AttackPressed = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputData.MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        _inputData.IsRunning = context.ReadValueAsButton();
    }
}