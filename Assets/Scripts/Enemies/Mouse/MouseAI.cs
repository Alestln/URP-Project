using UnityEngine;

public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // ��������������
        Chase // �������������
    }

    [Header("AI Logic")]
    [SerializeField] private float _chaseRadius = 3f; // ������ �������������
    [SerializeField] private Transform _target; // ���� ������������� (�����)

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // ��������� ��� �������� ����
    [SerializeField] private MouseAnimator _animator; // ��������� ��� �������� ����

    private MouseState _currentState;

    private void Start()
    {
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
        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        switch(_currentState)
        {
            case MouseState.Patrol:
                if (distanceToTarget < _chaseRadius)
                {
                    SwitchState(MouseState.Chase);
                }
                // ����� ����� �������� ������ ��������������, ��������, �������� �� ������
                break;
            case MouseState.Chase:
                if (distanceToTarget > _chaseRadius)
                {
                    SwitchState(MouseState.Patrol);
                }
                ExecuteChaseState();
                break;
        }
    }

    private void ExecuteChaseState()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
    }

    private void OnDrawGizmosSelected()
    {
        // ����������� ������� ������������� � ���������
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }
}