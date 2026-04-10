using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public ItemData[] items;
    private List<ItemData> inventory = new List<ItemData>();
    public SpriteRenderer itemHolder;
    private ItemData currentItemData;
    private GameObject addPrefInstance;
    private bool itemActive;

    public bool AddItem(ItemData item)
    {
        if (inventory.Count >= 1) return false;
        Debug.Log($"Adding item: {item.Name} to inventory.");
        inventory.Add(item);
        itemHolder.sprite = item.Icon;
        currentItemData = item;

        if (item.AdditionalPrefab != null)
        {
            addPrefInstance = Instantiate(item.AdditionalPrefab, transform);
            addPrefInstance = Instantiate(item.AdditionalPrefab, transform);
        }

        itemActive = false;
        return true;
    }

    public GameObject GetAddPref()
    {
        return addPrefInstance;
    }

    public bool IsItemActive()
    {
        return itemActive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inventory.Count > 0)
        {
            RemoveItem(inventory[0]);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemData currentItem = Getitem();
            if (currentItem != null)
                Debug.Log($"Current item in inventory: {currentItem.Name}");
            else
                Debug.Log("Inventory is empty.");
        }
    }

    public bool RemoveItem(ItemData item)
    {
        itemHolder.sprite = null;

        if (addPrefInstance != null)
        {
            Destroy(addPrefInstance);
            addPrefInstance = null;
        }

        currentItemData = null;
        GameObject p = Instantiate(item.Prefab, transform.position, Quaternion.identity);
        if (p.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        return inventory.Remove(item);
    }

    public ItemData Getitem() => inventory.Count > 0 ? inventory[0] : null;
}
