using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStats))]
public class MeleeAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("���������, ������� ������������ ��� ������ ��� ��������. �� ������ ���� Is Trigger.")]
    [SerializeField] private BoxCollider2D _hitboxTemplate;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private LayerMask _targetLayerMask;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    private static readonly int AttackTriggerHash = Animator.StringToHash(PlayerAnimationNames.Attack);

    [Header("Events")]
    public UnityEvent OnAttack;

    private CharacterStats _stats;
    private float _lastAttackTime;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        if (_hitboxTemplate == null)
        {
            Debug.LogError("�� �������� ������ �������� (Hitbox Template)!", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_hitboxTemplate != null)
        {
            _hitboxTemplate.enabled = false;
        }
    }

    public void Attack()
    {
        if (Time.time < _lastAttackTime + _attackCooldown) return;

        _lastAttackTime = Time.time;
        _animator.SetTrigger(AttackTriggerHash);
        OnAttack?.Invoke();

        Vector2 boxCenter = _hitboxTemplate.transform.position; // ����� ��������
        Vector2 boxSize = _hitboxTemplate.size; // ������ ��������
        float boxAngle = _hitboxTemplate.transform.eulerAngles.z; // ���� �������� ��������

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, boxAngle, _targetLayerMask);

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            {
                return; // ���������� ���� � ����� ��������
            }

            if (hit.TryGetComponent<CharacterStats>(out CharacterStats targetStats))
            {
                targetStats.TakeDamage(_stats.Damage);
            }
        }
    }
}
