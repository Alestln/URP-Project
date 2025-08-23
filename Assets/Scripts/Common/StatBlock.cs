using UnityEngine;

[CreateAssetMenu(menuName = "Stats/New Stat Block")]
public class StatBlock : ScriptableObject
{
    [Header("Core Stats")]
    public int MaxHealth = 100; // ������������ ��������
    public int Damage = 10; // ����
    public float MoveSpeed = 5f; // �������� ������������
    public float RunSpeed = 8f; // �������� ������������

    [Header("Defense Stats")]
    public int Armor = 0; // �����, ����������� ����
}