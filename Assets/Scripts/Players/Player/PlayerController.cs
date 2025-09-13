using System;
using UnityEngine;

namespace Assets.Scripts.Players.Player
{
    [RequireComponent(typeof(PlayerMover), typeof(CharacterStats))]
    [RequireComponent(typeof(MeleeAttacker))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _hitboxContainer;

        private PlayerMover _mover;
        private CharacterStats _stats;
        private PlayerInputData _inputData;
        private MeleeAttacker _meleeAttacker;

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
                TransformHelper.UpdateRotation(_hitboxContainer, _mover.MoveDirection);

                _meleeAttacker.Attack();
            }
        }

        internal void SetInput(PlayerInputData inputData)
        {
            _inputData = inputData;
        }
    }
}
