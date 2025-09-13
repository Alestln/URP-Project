using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class MeleeAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private BoxCollider2D _hitboxTemplate; // Collider used for detecting hits
    [SerializeField] private float _attackCooldown = 1.0f; // Time between attacks
    [SerializeField] private LayerMask _targetLayerMask; // Layers that can be hit

    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;
    private static readonly int AttackTriggerHash = Animator.StringToHash(PlayerAnimationNames.Attack);

    private CharacterStats _stats;
    private float _lastAttackTime = -999f;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        if (_hitboxTemplate == null)
        {
            Debug.LogError("Hitbox template is not assigned.", this);
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
        Debug.Log("MeleeAttacker: Attack initiated.");
        if (Time.time - _lastAttackTime < _attackCooldown)
        {
            return; // Still in cooldown
        }

        Debug.Log("MeleeAttacker: Performing attack.");

        _lastAttackTime = Time.time;
        _animator.SetTrigger(AttackTriggerHash);

        Vector2 boxCenter = _animator.transform.position;
        Vector2 boxSize = _hitboxTemplate.size;
        float boxAngle = _hitboxTemplate.transform.eulerAngles.z;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, boxAngle, _targetLayerMask);

        Debug.Log($"MeleeAttacker: Detected {hits.Length} hits.");

        foreach (var hit in hits)
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                continue; // Ignore self
            }

            if (hit.TryGetComponent<CharacterStats>(out CharacterStats targetStats))
            {
                targetStats.TakeDamage(_stats.Damage);
            }
        }
    }
}