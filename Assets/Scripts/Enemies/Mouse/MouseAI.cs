using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseMover), typeof(CharacterStats))]
public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // ��������������
        Chase // �������������
    }

    [Header("AI Logic")]
    [SerializeField] private float _patrolPointThreshold = 0.5f; // ����� ���������� �� ����� ��������������
    [SerializeField] private int _startPointIndex = 0; // ������ ��������� ����� ��������������, ���� ������������ ���� ��������������
    [SerializeField] private float _attackDistance = 1.3f; // ����������, �� ������� ���� ��������������� �� ���� ��� �������������

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // ��������� ��� �������� ����
    [SerializeField] private MouseAnimator _animator; // ��������� ��� �������� ����
    [SerializeField] private FieldOfView _fieldOfView; // ��������� ��� ���� ������ ����
    [SerializeField] private PatrolPath _patrolPath; // ���� �������������� ����

    [Header("Stats Block")]
    [SerializeField] private CharacterStats _stats; // ���� ���������� ����, ���������� �������� ��������������

    private MouseState _currentState;
    private int _currentPatrolIndex; // ������ ������� ����� ��������������
    private float _waitTimer = 0f; // ������ ��� �������� �� ������� ����� ��������������
    private Transform _currentTarget; // ������� ���� ��� �������������
    private readonly List<Transform> _visibleTargetsBuffer = new List<Transform>(); // ������ ������� �����

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
                transform.position = _patrolPath.GetPoint(_startPointIndex).Position; // ��������� ��������� ������� ����
                _currentPatrolIndex = _startPointIndex;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.LogError($"��������� ������ {_startPointIndex} �����������: {ex.Message}. ������� ����� ������� ��� � �����.");
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
            _currentTarget = _visibleTargetsBuffer[0]; // ���� ������ ������� ����
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
            _mover.SetMoveDirection(Vector2.zero); // �����
            _animator.SetDirection(direction);
            _fieldOfView.SetDirection(direction);
            return;
        }

        if (Vector2.Distance(transform.position, currentPoint.Position) < _patrolPointThreshold)
        {
            _waitTimer = currentPoint.WaitTime; // ������������� ����� ��������
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPath.Length;
            currentPoint = _patrolPath.GetPoint(_currentPatrolIndex); // ����� ����
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
            return; // ���� ���� �� ������, ������ �� ������
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