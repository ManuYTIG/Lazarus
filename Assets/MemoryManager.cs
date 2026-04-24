using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager instance;

    public int repairLevel = 0;

    void Awake()
    {
        instance = this;
    }

    public void OnRepair(int level)
    {
        repairLevel = level;

        if (level == 1)
        {
            // TODO: déclencher flash mémoire à la prochaine mort
            Debug.Log("[Memory] Réparation 1 — mémoire partielle débloquée");
        }
        else if (level == 2)
        {
            // TODO: cutscene pistolet
            Debug.Log("[Memory] Réparation 2 — souvenir de la barre débloqué");
        }
        else if (level == 3)
        {
            // TODO: événement réparation 3
            Debug.Log("[Memory] Réparation 3 — machine complète");
        }
    }
}