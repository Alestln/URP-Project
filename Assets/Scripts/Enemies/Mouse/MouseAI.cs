using System;
using UnityEngine;

[RequireComponent(typeof(MouseMover), typeof(MouseAnimator))]
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
    [SerializeField] private float _stoppingDistance = 0.1f; // ����������, �� ������� ���� ��������������� �� ���� ��� �������������

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // ��������� ��� �������� ����
    [SerializeField] private MouseAnimator _animator; // ��������� ��� �������� ����
    [SerializeField] private FieldOfView _fieldOfView; // ��������� ��� ���� ������ ����
    [SerializeField] private PatrolPath _patrolPath; // ���� �������������� ����

    private MouseState _currentState;
    private int _currentPatrolIndex; // ������ ������� ����� ��������������
    private float _waitTimer = 0f; // ������ ��� �������� �� ������� ����� ��������������

    private void Start()
    {
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
        RunFSM(); // ������ ��������� �������� ��������� (FSM) ��� ����
        // ����� ����� �������� �������������� �������� ��� ������, ���� �����
        // ��������, �������� �� ��������, �������������� � ���������� � �.�.
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
            _mover.SetMoveDirection(Vector2.zero); // �����
            _animator.SetDirection(Vector2.zero);
            _fieldOfView.SetDirection(Vector2.zero);
            return;
        }

        if (Vector2.Distance(transform.position, currentPoint.Position) < _patrolPointThreshold)
        {
            _waitTimer = currentPoint.WaitTime; // ������������� ����� ��������
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPath.Length;
            currentPoint = _patrolPath.GetPoint(_currentPatrolIndex); // ����� ����
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
            return; // ���� ���� �� ������, ������ �� ������
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