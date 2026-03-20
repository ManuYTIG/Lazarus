using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public Sprite Icon;
}
