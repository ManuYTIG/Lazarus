using UnityEngine;

public class RepairMachine : MonoBehaviour
{
    [Header("Repair Settings")]
    public int hpBonusPerRepair = 2;

    private bool playerInRange = false;
    private PlayerController player;
    private Health playerHealth;

    // État des réparations
    private bool coreInstalled = false;
    private bool stickInstalled = false;
    private bool thirdInstalled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerController>();
            playerHealth = other.GetComponent<Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerHealth = null;
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemData item = player.inventorySystem.Getitem();
            if (item == null) return;

            if (item.ID == "core_item" && !coreInstalled)
                InstallCore(item);
            else if (item.ID == "light_item" && !stickInstalled && coreInstalled)
                InstallStick(item);
            else if (item.ID == "third_item" && !thirdInstalled && stickInstalled)
                InstallThird(item);
        }
    }

    private void InstallCore(ItemData item)
    {
        player.inventorySystem.RemoveItem(item);
        coreInstalled = true;
        ApplyHPBonus();
        CameraFollow.instance?.TriggerShake(0.4f, 0.2f);
        MemoryManager.instance?.OnRepair(1);
        Debug.Log("[Machine] Core installé — Réparation 1");
        // TODO: illuminer slot barre
    }

    private void InstallStick(ItemData item)
    {
        player.inventorySystem.RemoveItem(item);
        stickInstalled = true;
        ApplyHPBonus();
        CameraFollow.instance?.TriggerShake(0.4f, 0.2f);
        MemoryManager.instance?.OnRepair(2);
        Debug.Log("[Machine] Barre installée — Réparation 2");
    }

    private void InstallThird(ItemData item)
    {
        player.inventorySystem.RemoveItem(item);
        thirdInstalled = true;
        ApplyHPBonus();
        CameraFollow.instance?.TriggerShake(0.4f, 0.2f);
        MemoryManager.instance?.OnRepair(3);
        Debug.Log("[Machine] Troisième item installé — Réparation 3");
    }

    private void ApplyHPBonus()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth += hpBonusPerRepair;
            playerHealth.ResetHealth();
        }
    }
}
