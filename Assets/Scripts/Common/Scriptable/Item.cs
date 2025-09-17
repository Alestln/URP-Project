using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string Name; // �������� ��������
    public Sprite Icon; // ������ ��������
    public string Description; // �������� ��������
}