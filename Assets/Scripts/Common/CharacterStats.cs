using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    private const int MinHealth = 0; // Минимальное здоровье противника

    [SerializeField] private StatBlock _baseStats; // Базовые характеристики противника

    public int MaxHealth { get; private set; }
    public int CurrentHealts { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public int Armor { get; private set; }

    public UnityEvent<int, int> OnHealthChanged; // Событие для отслеживания изменений здоровья
    public UnityEvent OnDied; // Событие для отслеживания смерти противника

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
        Armor = _baseStats.Armor;
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Max(0, damage - Armor);

        CurrentHealts -= damageTaken;
        CurrentHealts = Mathf.Clamp(CurrentHealts, MinHealth, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealts, MaxHealth);
        Debug.Log($"{gameObject.name} получил {damageTaken} урона. Осталось здоровья: {CurrentHealts}");

        if (CurrentHealts <= MinHealth)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} умер.");
        OnDied?.Invoke();

        gameObject.SetActive(false); // Деактивируем противника
    }
}
