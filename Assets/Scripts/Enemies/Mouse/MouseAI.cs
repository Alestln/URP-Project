using System;
using UnityEngine;

public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // Патрулирование
        Chase // Преследование
    }

    [Header("AI Logic")]
    [SerializeField] private float _patrolPointThreshold = 0.5f; // Порог расстояния до точки патрулирования
    [SerializeField] private Transform[] _patrolPoints; // Точки патрулирования

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // Компонент для движения мыши
    [SerializeField] private MouseAnimator _animator; // Компонент для анимации мыши
    [SerializeField] private FieldOfView _fieldOfView; // Компонент для поля зрения мыши (если используется)

    private MouseState _currentState;
    private int _currentPatrolIndex = 0; // Индекс текущей точки патрулирования

    private void Start()
    {
        if (_patrolPoints.Length > 0)
        {
            transform.position = _patrolPoints[0].position; // Установка начальной позиции на первую точку патрулирования
        }

        _currentState = MouseState.Patrol;
    }

    private void SwitchState(MouseState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        _currentState = newState;
    }

    private void Update()
    {
        RunFSM(); // Запуск конечного автомата состояний (FSM) для мыши
        // Здесь можно добавить дополнительные проверки или логику, если нужно
        // Например, проверка на здоровье, взаимодействие с окружением и т.д.
    }

    private void RunFSM()
    {
        switch(_currentState)
        {
            case MouseState.Patrol:
                if (_fieldOfView.IsTargetVisible())
                {
                    SwitchState(MouseState.Chase);
                }
                ExecutePatrolState();
                break;
            case MouseState.Chase:
                if (!_fieldOfView.IsTargetVisible())
                {
                    SwitchState(MouseState.Patrol);
                }
                ExecuteChaseState();
                break;
        }
    }

    private void ExecutePatrolState()
    {
        Vector2 direction = Vector2.zero;

        if (_patrolPoints.Length > 1)
        {
            Transform targetPoint = _patrolPoints[_currentPatrolIndex];

            if (Vector2.Distance(transform.position, targetPoint.position) < _patrolPointThreshold)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length; // Переход к следующей точке патрулирования
            }

            direction = (targetPoint.position - transform.position).normalized;
        }

        if (direction != Vector2.zero)
        {
            _mover.SetMoveDirection(direction);
            _animator.SetDirection(direction);
        }
    }

    private void ExecuteChaseState()
    {
        Vector2 direction = (_fieldOfView.Target.position - transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
    }
}