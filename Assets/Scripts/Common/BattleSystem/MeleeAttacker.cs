using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStats))]
public class MeleeAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform _attackPoint; // Точка, из которой исходит атака
    [SerializeField] private float _attackRadius = 0.5f; // Радиус атаки
    [SerializeField] private float _attackCooldown = 1f; // Время перезарядки между атаками
    [SerializeField] private LayerMask _targetLayerMask; // Кого мы можем атаковать

    [Header("Animation")]
    [SerializeField] private Animator _animator; // Аниматор для воспроизведения анимаций атаки
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

    [Header("Events")]
    public UnityEvent OnAttack; // Событие, вызываемое при атаке

    private CharacterStats _stats;
    private float _lastAttackTime;

    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
        if (_attackPoint == null)
        {
            _attackPoint = transform; // Если точка атаки не задана, используем позицию объекта
        }
    }

    public void Attack()
    {
        // 1. Проверяем кулдаун
        if (Time.time < _lastAttackTime + _attackCooldown)
        {
            return; // Еще не прошло время перезарядки
        }

        _lastAttackTime = Time.time;

        // 2. Запускаем анимацию и событие
        _animator.SetTrigger(AttackTriggerHash);
        OnAttack?.Invoke();

        // 3. Находим цели в зоне атаки
        Collider2D[] hits = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _targetLayerMask);

        // 4. Наносим урон каждой цели
        foreach (var hit in hits)
        {
            if (hit.transform == transform)
            {
                continue; // Пропускаем себя
            }

            if (hit.TryGetComponent<CharacterStats>(out CharacterStats targetStats))
            {
                Debug.Log($"{gameObject.name} атакует {hit.gameObject.name} на {_stats.Damage} урона.");
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