using UnityEngine;

namespace Assets.Scripts.Players.Player
{
    [RequireComponent(typeof(PlayerMover), typeof(CharacterStats))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMover _mover;
        private CharacterStats _stats;
        private PlayerInputData _inputData;

        public bool CanControl { get; set; } = true;

        private void Awake()
        {
            _mover = GetComponent<PlayerMover>();
            _stats = GetComponent<CharacterStats>();
        }

        private void Update()
        {
            if (!CanControl)
            {
                _mover.Stop();
                return;
            }

            HandleMovement();
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

        internal void SetInput(PlayerInputData inputData)
        {
            _inputData = inputData;
        }
    }
}
