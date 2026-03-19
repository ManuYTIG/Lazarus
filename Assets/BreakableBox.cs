using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject itemToSpawn;

    public void BreakBox()
    {
        if (itemToSpawn != null)
        {
            Vector3 spawnPos = gameObject.transform.position;
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
