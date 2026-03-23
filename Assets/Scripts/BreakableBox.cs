using System.Collections;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject itemToSpawn;
    public GameObject particlePrefab;
    public int health;
    public Sprite[] damagedSprites; // Array of sprites for different damage states
    public float particleLife;
    public float recoilForce;
    public float wobbleScale = 0.9f;   // how much it squashes
    public float wobbleTime = 0.1f;    // how fast it wobbles
    public float damageCooldown = 0.2f;

    private bool canTakeDamage = true;
    private bool isWobbling = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void ApplyRecoil(Collision2D collision)
    {
        // Direction: from enemy box
        Vector2 direction = (transform.position - collision.transform.position).normalized;

        // Apply impulse
        rb.AddForce(direction * recoilForce, ForceMode2D.Impulse);
    }
    private IEnumerator Wobble()
    {
        if (isWobbling) yield break;
        isWobbling = true;

        Vector3 originalScale = transform.localScale;
        Vector3 squashed = new Vector3(wobbleScale, 1f / wobbleScale, 1f);

        // Squash
        transform.localScale = squashed;
        yield return new WaitForSeconds(wobbleTime);

        // Stretch
        transform.localScale = new Vector3(1f / wobbleScale, wobbleScale, 1f);
        yield return new WaitForSeconds(wobbleTime);

        // Reset
        transform.localScale = originalScale;

        isWobbling = false;
    }
    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canTakeDamage) return;

        if (collision.gameObject.CompareTag("Enemy 1"))
        {
            StartCoroutine(DamageCooldown());
            StartCoroutine(Wobble());

            if (health > 0)
            {
                health--;
                gameObject.GetComponent<SpriteRenderer>().sprite = damagedSprites[health];
                SpawnParticles();
                ApplyRecoil(collision); // from previous step
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
