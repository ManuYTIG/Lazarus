using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] private string[] tagImmune;
    [SerializeField] private float damageValue;
    [SerializeField] private float damageInterval;
    [SerializeField] private bool isProjectile;

    private Health targetHealth;
    private float nextDamageTime;
    private bool touchingTarget;

    public GameObject particleSpawn;
    public float particleTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore immune targets
        for(int i = 0; i < tagImmune.Length; i++)
        {
            if (collision.gameObject.CompareTag(tagImmune[i]))
                return;
        }
        if (collision.gameObject.CompareTag("Larva Egg"))
        {
            if(collision.gameObject.TryGetComponent(out LarvaScript egg))
            {
                egg.BreakEgg(false);
            }
        }

        // Check if the collided object has health
        if (collision.gameObject.TryGetComponent(out Health temp))
        {
            targetHealth = temp;
            touchingTarget = true;
            nextDamageTime = Time.time + damageInterval; // ← add this line
            // Deal immediate damage on first contact
            ApplyDamage();
        }

        // Projectiles damage once and disappear
        if (isProjectile)
        {
            SpawnParticle();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tagImmune.Length; i++)
            if (collision.gameObject.CompareTag(tagImmune[i])) return;

        if (collision.gameObject.CompareTag("Larva Egg"))
            if (collision.gameObject.TryGetComponent(out LarvaScript egg))
                egg.BreakEgg(false);

        if (collision.gameObject.TryGetComponent(out Health temp))
        {
           targetHealth = temp;
            touchingTarget = true;
            nextDamageTime = Time.time + damageInterval; // ← set cooldown BEFORE first hit
            ApplyDamage();
        }

        if (isProjectile)
        {
            SpawnParticle();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Only apply damage if still touching AND cooldown passed
        if (touchingTarget && targetHealth != null && Time.time >= nextDamageTime)
        {
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        targetHealth.RemoveHealth(damageValue);
        nextDamageTime = Time.time + damageInterval;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Only stop damaging if THIS exact object is the one we were hitting
        if (targetHealth != null && collision.gameObject == targetHealth.gameObject)
        {
            touchingTarget = false;
            targetHealth = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only stop damaging if THIS exact object is the one we were hitting
        if (targetHealth != null && collision.gameObject == targetHealth.gameObject)
        {
            touchingTarget = false;
            targetHealth = null;
        }
    }
    private void SpawnParticle()
    {
        GameObject p = Instantiate(particleSpawn, transform.position, Quaternion.identity);
        Destroy(p, particleTime);
    }
}