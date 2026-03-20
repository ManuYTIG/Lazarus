using System.Collections.Generic;
using UnityEngine;

//One item inventory system that can hold only one item at a time. It allows adding, removing, and retrieving the item in the inventory.
public class InventorySystem : MonoBehaviour
{
    public ItemData[] items;
    private List<ItemData> inventory = new List<ItemData>();

    public bool AddItem(ItemData item)
    {
        if (inventory.Count >= 1) return false; // Inventory is full, cannot add more items
        Debug.Log($"Adding item: {item.Name} to inventory.");
        inventory.Add(item);
        return true;
    }

    //private void Start()
    //{
        // Example of adding an item to the inventory at the start
    //    if (items.Length > 0)
    //    {
    //        AddItem(items[0]);
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inventory.Count > 0)
        {
            RemoveItem(inventory[0]);
        }
        //Example of retrieving the current item in the inventory when the player presses the 'I' key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemData currentItem = Getitem();
            if (currentItem != null)
            {
                Debug.Log($"Current item in inventory: {currentItem.Name}");
            }
            else
            {
                Debug.Log("Inventory is empty.");
            }
        }
      
    }

    public bool RemoveItem(ItemData item)
    {
        return inventory.Remove(item);
    }

    public ItemData Getitem() => inventory.Count > 0 ? inventory[0] : null; // Return the first item in the inventory or null if empty
}
