using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStats))]
public class MeleeAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("Коллайдер, который используется как шаблон для хитбокса. Он должен быть Is Trigger.")]
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
            Debug.LogError("Не назначен шаблон хитбокса (Hitbox Template)!", this);
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

        Vector2 boxCenter = _hitboxTemplate.transform.position; // Центр хитбокса
        Vector2 boxSize = _hitboxTemplate.size; // Размер хитбокса
        float boxAngle = _hitboxTemplate.transform.eulerAngles.z; // Угол поворота хитбокса

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, boxAngle, _targetLayerMask);

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            {
                return; // Игнорируем себя и своих потомков
            }

            if (hit.TryGetComponent<CharacterStats>(out CharacterStats targetStats))
            {
                targetStats.TakeDamage(_stats.Damage);
            }
        }
    }
}
