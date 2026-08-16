using UnityEngine;

[CreateAssetMenu (fileName ="InventoryItem", menuName ="NewInventoryItem")]

public class InventoryItem : ScriptableObject
{
    [Header("General Info")]
    public int idInventory;
    public string inventoryDescription;
    public string inventoryName;
    public Sprite inventoryImage;

}
