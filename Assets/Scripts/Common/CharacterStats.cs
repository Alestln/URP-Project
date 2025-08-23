using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    private const int MinHealth = 0; // Минимальное значение здоровья

    [SerializeField] private StatBlock _baseStats; // Базовые характеристики противника

    public int MaxHealth { get; private set; }
    public int CurrentHealts { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public int Armor { get; private set; }

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDied;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        MaxHealth = _baseStats.MaxHealth;
        CurrentHealts = _baseStats.MaxHealth;
        Damage = _baseStats.Damage;
        MoveSpeed = _baseStats.MoveSpeed;
        RunSpeed = _baseStats.RunSpeed;
        Armor = _baseStats.Armor;
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Max(0, damage - Armor);

        CurrentHealts = Mathf.Clamp(CurrentHealts - damageTaken, MinHealth, MaxHealth);

        Debug.Log($"{gameObject.name} took {damageTaken} damage, current health: {CurrentHealts}/{MaxHealth}");
        OnHealthChanged?.Invoke(CurrentHealts, MaxHealth);

        if (CurrentHealts <= MinHealth)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke();

        gameObject.SetActive(false);
    }
}
