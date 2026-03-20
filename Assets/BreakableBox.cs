using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject itemToSpawn;
    public GameObject particlePrefab;
    public int health = 3;
    public float particleLife;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy 1"))
        {
            if(health > 0)
            {
                health--;
            }
            else
            {
                BreakBox();
            }
            
        }
    }

    public void BreakBox()
    {
        if (itemToSpawn != null)
        {
            SpawnParticles();
            Vector3 spawnPos = gameObject.transform.position;
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public void SpawnParticles()
    {
        GameObject p = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(p, particleLife); // destroy after 2 seconds (or particle duration)
    }

}
