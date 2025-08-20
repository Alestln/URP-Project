using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    private const int MinHealth = 0; // ����������� �������� ����������

    [SerializeField] private StatBlock _baseStats; // ������� �������������� ����������

    public int MaxHealth { get; private set; }
    public int CurrentHealts { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public int Armor { get; private set; }

    public UnityEvent<int, int> OnHealthChanged; // ������� ��� ������������ ��������� ��������
    public UnityEvent OnDied; // ������� ��� ������������ ������ ����������

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
        Debug.Log($"{gameObject.name} ������� {damageTaken} �����. �������� ��������: {CurrentHealts}");

        if (CurrentHealts <= MinHealth)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ����.");
        OnDied?.Invoke();

        gameObject.SetActive(false); // ������������ ����������
    }
}
