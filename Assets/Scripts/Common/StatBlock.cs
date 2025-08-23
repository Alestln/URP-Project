using UnityEngine;

[CreateAssetMenu(menuName = "Stats/New Stat Block")]
public class StatBlock : ScriptableObject
{
    [Header("Core Stats")]
    public int MaxHealth = 100; // Максимальное здоровье
    public int Damage = 10; // Урон
    public float MoveSpeed = 5f; // Скорость передвижения
    public float RunSpeed = 8f; // Скорость передвижения

    [Header("Defense Stats")]
    public int Armor = 0; // Броня, уменьшающая урон
}