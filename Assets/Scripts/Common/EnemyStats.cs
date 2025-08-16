using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private StatBlock _baseStats; // Базовые характеристики противника

    public int MaxHealth { get; private set; }
    public int CurrentHealts { get; private set; }
    public int Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public int Armor { get; private set; }

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
}
