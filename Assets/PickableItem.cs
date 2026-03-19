using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public ItemData itemData; // Reference to the item data for this pickable item
    private bool isPlayerInRange = false; // Flag to check if the player is within range to pick up the item
    private InventorySystem inventorySystem; // Reference to the player's inventory system

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E)) // Check if the player is in range and presses the 'E' key
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Player in range of item");
        if (other.CompareTag("Player")) // Check if the player enters the trigger area
        {
            isPlayerInRange = true; // Set the flag to true
            inventorySystem = other.GetComponent<InventorySystem>(); // Get the player's inventory system
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player exits the trigger area
        {
            isPlayerInRange = false; // Set the flag to false
            inventorySystem = null; // Clear the reference to the inventory system
        }
    }

    private void PickUp()
    {
        if (inventorySystem != null && inventorySystem.AddItem(itemData)) // Try to add the item to the inventory
        {
            Destroy(gameObject); // Destroy the pickable item from the scene if it was successfully added to the inventory
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
}
