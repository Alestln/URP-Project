using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    private PlayerMover _playerMover;
    private CharacterStats _stats;

    private PlayerInputData _inputData;

    public bool CanControl { get; set; } = true;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _stats = GetComponent<CharacterStats>();
    }

    internal void SetInput(PlayerInputData inputData)
    {
        _inputData = inputData;
    }

    private void Update()
    {
        if (!CanControl)
        {
            _playerMover.Stop();
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_inputData.MoveDirection != Vector2.zero)
        {
            float targetSpeed = _inputData.IsRunning ? _stats.RunSpeed : _stats.MoveSpeed;
            _playerMover.Move(_inputData.MoveDirection, targetSpeed);
        }
        else
        {
            _playerMover.Stop();
        }
    }
}