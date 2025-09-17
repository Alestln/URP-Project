using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string Name; // Название предмета
    public Sprite Icon; // Иконка предмета
    public string Description; // Описание предмета
}