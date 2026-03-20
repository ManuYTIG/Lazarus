using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] private string tagImmune = "Player";
    [SerializeField] private float damageValue = 10f;
    [SerializeField] private float damageInterval = 1.0f; // Damage every 1 second
    public bool isProjectile;
    private Health targetHealth;
    private float nextDamageTime;
    public GameObject particleSpawn;
    public float particleTime;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // Check immunity and get the Health component once
        if (collision.gameObject.CompareTag(tagImmune)) return;

        if (collision.gameObject.TryGetComponent(out Health temp)) 
        {
            targetHealth = temp;
            Debug.Log("collided" + targetHealth.gameObject);
            // Deal damage immediately on impact
            ApplyDamage();
        }
        if(isProjectile)
        {
            SpawnParticle();
            Destroy(gameObject);
        }
    }

    private void Update() 
    {
        // If we are touching something with health and enough time has passed
        if (targetHealth != null && Time.time >= nextDamageTime) 
        {
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        targetHealth.RemoveHealth(damageValue);
        nextDamageTime = Time.time + damageInterval; // Reset the "cooldown"
        Debug.Log("damaged " + targetHealth.gameObject + ", Health: " + targetHealth.health);
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        // When we stop touching it, stop the damage
        targetHealth = null;
    }
    private void SpawnParticle(){
        GameObject p = Instantiate(particleSpawn, transform.position, Quaternion.identity);
        Destroy(p, particleTime);
    }
}