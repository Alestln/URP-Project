using System;
using UnityEngine;

namespace Assets.Scripts.Players.Player
{
    [RequireComponent(typeof(PlayerMover), typeof(CharacterStats))]
    [RequireComponent(typeof(MeleeAttacker))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMover _mover;
        private CharacterStats _stats;
        private PlayerInputData _inputData;
        private MeleeAttacker _meleeAttacker;

        [Header("Combat")]
        [Tooltip("Контейнер с хитбоксами, который нужно поворачивать.")]
        [SerializeField] private Transform _hitboxContainer;

        public bool CanControl { get; set; } = true;

        private void Awake()
        {
            _mover = GetComponent<PlayerMover>();
            _stats = GetComponent<CharacterStats>();
            _meleeAttacker = GetComponent<MeleeAttacker>();
        }

        private void Update()
        {
            if (!CanControl)
            {
                _mover.Stop();
                return;
            }

            HandleMovement();
            HandleCombat();
        }

        private void HandleMovement()
        {
            if (_inputData.MoveDirection != Vector2.zero)
            {
                float targetSpeed = _inputData.IsRunning ? _stats.RunSpeed : _stats.MoveSpeed;
                _mover.Move(_inputData.MoveDirection, targetSpeed);
            }
            else
            {
                _mover.Stop();
            }
        }

        private void HandleCombat()
        {
            if (_inputData.AttackPressed)
            {
                UpdateHitboxRotation(_mover.MoveDirection);
                _meleeAttacker.Attack();
            }
        }

        private void UpdateHitboxRotation(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            float angle = Vector2.SignedAngle(Vector2.up, direction);
            _hitboxContainer.rotation = Quaternion.Euler(0, 0, angle);
        }

        internal void SetInput(PlayerInputData inputData)
        {
            _inputData = inputData;
        }
    }
}
