using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseMover), typeof(CharacterStats))]
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
    [SerializeField] private float _attackDistance = 1.3f; // Расстояние, на котором мышь останавливается от цели при преследовании

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // Компонент для движения мыши
    [SerializeField] private MouseAnimator _animator; // Компонент для анимации мыши
    [SerializeField] private FieldOfView _fieldOfView; // Компонент для поля зрения мыши
    [SerializeField] private PatrolPath _patrolPath; // Путь патрулирования мыши

    [Header("Stats Block")]
    [SerializeField] private CharacterStats _stats; // Блок статистики мыши, содержащий основные характеристики

    private MouseState _currentState;
    private int _currentPatrolIndex; // Индекс текущей точки патрулирования
    private float _waitTimer = 0f; // Таймер для ожидания на текущей точке патрулирования
    private Transform _currentTarget; // Текущая цель для преследования
    private readonly List<Transform> _visibleTargetsBuffer = new List<Transform>(); // Список видимых целей

    private void Awake()
    {
        _mover = GetComponent<MouseMover>();
        _animator = GetComponentInChildren<MouseAnimator>();
        _fieldOfView = GetComponentInChildren<FieldOfView>();
        _stats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        _mover.SetSpeed(_stats.MoveSpeed);

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
        UpdateTarget();
        RunFSM();
    }

    private void UpdateTarget()
    {
        _fieldOfView.FindVisibleTargets(_visibleTargetsBuffer);

        if (_visibleTargetsBuffer.Count > 0)
        {
            _currentTarget = _visibleTargetsBuffer[0]; // Берём первую видимую цель
            SwitchState(MouseState.Chase);
        }
        else if (_currentTarget != null)
        {
            _currentTarget = null;
            SwitchState(MouseState.Patrol);
        }
    }

    private void RunFSM()
    {
        switch(_currentState)
        {
            case MouseState.Patrol:
                ExecutePatrolState();
                break;
            case MouseState.Chase:
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
        Vector2 direction = (currentPoint.Position - (Vector2)transform.position).normalized;

        if (_waitTimer > 0f)
        {
            _waitTimer -= Time.deltaTime;
            _mover.SetMoveDirection(Vector2.zero); // Стоим
            _animator.SetDirection(direction);
            _fieldOfView.SetDirection(direction);
            return;
        }

        if (Vector2.Distance(transform.position, currentPoint.Position) < _patrolPointThreshold)
        {
            _waitTimer = currentPoint.WaitTime; // Устанавливаем время ожидания
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPath.Length;
            currentPoint = _patrolPath.GetPoint(_currentPatrolIndex); // Новая цель
        }

        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
        _fieldOfView.SetDirection(direction);
    }

    private void ExecuteChaseState()
    {
        if (_currentTarget == null)
        {
            SwitchState(MouseState.Patrol);
            return; // Если цель не задана, ничего не делаем
        }

        float distanceToTarget = Vector2.Distance(transform.position, _currentTarget.position);
        Vector2 direction = (_currentTarget.position - transform.position).normalized;

        if (distanceToTarget > _attackDistance)
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