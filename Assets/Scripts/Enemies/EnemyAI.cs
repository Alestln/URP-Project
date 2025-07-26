using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol, // ��������������
        Chase   // �������������
    }

    [Header("AI Logic")]
    [Tooltip("������, � ������� ���� �������� ������ � �������� �������������.")]
    [SerializeField] private float _chaseRadius = 7f;
    [Tooltip("������ �� ��������� ������. ������ ���� ����������� � ���������� �� �����.")]
    [SerializeField] private Transform _playerTransform;

    [Header("Patrol Settings")]
    [Tooltip("������ �����, ����� �������� ���� ����� �������������.")]
    [SerializeField] private Transform[] _patrolPoints;
    [Tooltip("��������� ������ ���� ������ ������� � ����� �������, ����� ������� �� �����������.")]
    [SerializeField] private float _patrolPointReachedThreshold = 0.5f;

    [Header("Component References")]
    [SerializeField] private MouseMover _mover;
    [SerializeField] private MouseAnimator _animator;

    private EnemyState _currentState;
    private int _currentPatrolPointIndex;

    private void Start()
    {
        // ���������: ���������, ��� �� ��������� ���������.
        if (_playerTransform == null || _mover == null || _animator == null)
        {
            Debug.LogError("������ ������������ EnemyAI! �� ��� ������ �����������.", this);
            enabled = false; // ��������� ���������, ����� �������� ������ � ��������.
            return;
        }

        // ������������� ��������� ���������
        _currentState = EnemyState.Patrol;
    }

    private void Update()
    {
        // �������� ���� �������� �������
        RunFSM();
    }

    private void RunFSM()
    {
        // ���������� �� ������ ����������� ���� ��� �� ���� ��� �������� ��������
        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        // ������ ������������ ���������
        switch (_currentState)
        {
            case EnemyState.Patrol:
                // ���� ����� ����� � ������ ����, ������������� �� �������������
                if (distanceToPlayer < _chaseRadius)
                {
                    SwitchState(EnemyState.Chase);
                }
                ExecutePatrolState();
                break;

            case EnemyState.Chase:
                // ���� ����� ����� �� �������, ������������ � �������
                if (distanceToPlayer > _chaseRadius)
                {
                    SwitchState(EnemyState.Patrol);
                }
                ExecuteChaseState();
                break;
        }
    }

    private void SwitchState(EnemyState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }

    // ���������� ������ ��������������
    private void ExecutePatrolState()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _mover.Stop(); // ���� ������������� �����, ����� �� �����
            _animator.SetDirection(Vector2.down); // ������� ���� ��� �������
            return;
        }

        Transform targetPoint = _patrolPoints[_currentPatrolPointIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction); // ��������� ����������� ��� ���������

        // ���� �������� ����� �������, �������� ���������
        if (Vector2.Distance(transform.position, targetPoint.position) < _patrolPointReachedThreshold)
        {
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % _patrolPoints.Length;
        }
    }

    // ���������� ������ �������������
    private void ExecuteChaseState()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
    }

    // ������������ ������� ������������� � ��������� ��� ������� ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }
}
