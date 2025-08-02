using System;
using UnityEngine;

public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // ��������������
        Chase // �������������
    }

    [Header("AI Logic")]
    [SerializeField] private float _patrolPointThreshold = 0.5f; // ����� ���������� �� ����� ��������������
    [SerializeField] private Transform[] _patrolPoints; // ����� ��������������

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // ��������� ��� �������� ����
    [SerializeField] private MouseAnimator _animator; // ��������� ��� �������� ����
    [SerializeField] private FieldOfView _fieldOfView; // ��������� ��� ���� ������ ���� (���� ������������)

    private MouseState _currentState;
    private int _currentPatrolIndex = 0; // ������ ������� ����� ��������������

    private void Start()
    {
        if (_patrolPoints.Length > 0)
        {
            transform.position = _patrolPoints[0].position; // ��������� ��������� ������� �� ������ ����� ��������������
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
        Vector2 direction = Vector2.zero;

        if (_patrolPoints.Length > 1)
        {
            Transform targetPoint = _patrolPoints[_currentPatrolIndex];

            if (Vector2.Distance(transform.position, targetPoint.position) < _patrolPointThreshold)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length; // ������� � ��������� ����� ��������������
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