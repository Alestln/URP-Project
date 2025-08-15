using System;
using UnityEngine;

[RequireComponent(typeof(MouseMover), typeof(MouseAnimator))]
public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // Патрулирование
        Chase // Преследование
    }

    [Header("AI Logic")]
    [SerializeField] private float _patrolPointThreshold = 0.5f; // Порог расстояния до точки патрулирования
    [SerializeField] private int _startPointIndex = 0; // Индекс начальной точки патрулирования, если используется путь патрулирования
    [SerializeField] private float _stoppingDistance = 0.1f; // Расстояние, на котором мышь останавливается от цели при преследовании

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // Компонент для движения мыши
    [SerializeField] private MouseAnimator _animator; // Компонент для анимации мыши
    [SerializeField] private FieldOfView _fieldOfView; // Компонент для поля зрения мыши
    [SerializeField] private PatrolPath _patrolPath; // Путь патрулирования мыши

    private MouseState _currentState;
    private int _currentPatrolIndex; // Индекс текущей точки патрулирования
    private float _waitTimer = 0f; // Таймер для ожидания на текущей точке патрулирования

    private void Start()
    {
        if (_patrolPath is not null && _patrolPath.Length > 1)
        {
            try
            {
                transform.position = _patrolPath.GetPoint(_startPointIndex).Position; // Установка начальной позиции мыши
                _currentPatrolIndex = _startPointIndex;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.LogError($"Стартовый индекс {_startPointIndex} некорректен: {ex.Message}. Позиция врага остаётся как в сцене.");
                return;
            }
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
        if (_patrolPath is null || _patrolPath.Length <= 1)
        {
            return;
        }

        PatrolPoint currentPoint = _patrolPath.GetPoint(_currentPatrolIndex);

        if (_waitTimer > 0f)
        {
            _waitTimer -= Time.deltaTime;
            _mover.SetMoveDirection(Vector2.zero); // Стоим
            _animator.SetDirection(Vector2.zero);
            _fieldOfView.SetDirection(Vector2.zero);
            return;
        }

        if (Vector2.Distance(transform.position, currentPoint.Position) < _patrolPointThreshold)
        {
            _waitTimer = currentPoint.WaitTime; // Устанавливаем время ожидания
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPath.Length;
            currentPoint = _patrolPath.GetPoint(_currentPatrolIndex); // Новая цель
        }

        Vector2 direction = (currentPoint.Position - (Vector2)transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
        _fieldOfView.SetDirection(direction);
    }

    private void ExecuteChaseState()
    {
        if (_fieldOfView.Target is null)
        {
            return; // Если цель не задана, ничего не делаем
        }

        float distanceToTarget = Vector2.Distance(transform.position, _fieldOfView.Target.position);
        print(distanceToTarget);
        Vector2 direction = (_fieldOfView.Target.position - transform.position).normalized;

        if (distanceToTarget > _stoppingDistance)
        {
            _mover.SetMoveDirection(direction);
        }
        else
        {
            _mover.SetMoveDirection(Vector2.zero);
        }

        _animator.SetDirection(direction);
    }
}