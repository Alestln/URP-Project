using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    private const int MinHealth = 0; // Минимальное значение здоровья

    [SerializeField] private StatBlock _baseStats; // Базовые характеристики противника

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public int Armor { get; private set; }

    public bool IsInvincible { get; set; } = false;

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDied;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        MaxHealth = _baseStats.MaxHealth;
        CurrentHealth = _baseStats.MaxHealth;
        Damage = _baseStats.Damage;
        MoveSpeed = _baseStats.MoveSpeed;
        RunSpeed = _baseStats.RunSpeed;
        Armor = _baseStats.Armor;
    }

    public void TakeDamage(int damage)
    {
        if (IsInvincible)
        {
            return;
        }

        int damageTaken = Mathf.Max(0, damage - Armor);
        CurrentHealth = Mathf.Clamp(CurrentHealth - damageTaken , 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        Debug.Log($"{gameObject.name} получил {damageTaken} урона. Осталось здоровья: {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} умер.");

        OnDied?.Invoke();
        gameObject.SetActive(false);
    }
}
