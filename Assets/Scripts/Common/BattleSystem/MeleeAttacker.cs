using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStats))]
public class MeleeAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform _attackPoint; // �����, �� ������� ������� �����
    [SerializeField] private float _attackRadius = 0.5f; // ������ �����
    [SerializeField] private float _attackCooldown = 1f; // ����� ����������� ����� �������
    [SerializeField] private LayerMask _targetLayerMask; // ���� �� ����� ���������

    [Header("Animation")]
    [SerializeField] private Animator _animator; // �������� ��� ��������������� �������� �����
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

    [Header("Events")]
    public UnityEvent OnAttack; // �������, ���������� ��� �����

    private CharacterStats _stats;
    private float _lastAttackTime;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        if (_attackPoint == null)
        {
            _attackPoint = transform; // ���� ����� ����� �� ������, ���������� ������� �������
        }
    }

    public void Attack()
    {
        // 1. ��������� �������
        if (Time.time < _lastAttackTime + _attackCooldown)
        {
            return; // ��� �� ������ ����� �����������
        }

        _lastAttackTime = Time.time;

        // 2. ��������� �������� � �������
        _animator.SetTrigger(AttackTriggerHash);
        OnAttack?.Invoke();

        // 3. ������� ���� � ���� �����
        Collider2D[] hits = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _targetLayerMask);

        // 4. ������� ���� ������ ����
        foreach (var hit in hits)
        {
            if (hit.transform == transform)
            {
                continue; // ���������� ����
            }

            if (hit.TryGetComponent<CharacterStats>(out CharacterStats targetStats))
            {
                Debug.Log($"{gameObject.name} ������� {hit.gameObject.name} �� {_stats.Damage} �����.");
                targetStats.TakeDamage(_stats.Damage);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
#endif
}