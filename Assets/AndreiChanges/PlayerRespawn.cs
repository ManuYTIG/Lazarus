using UnityEngine;
public class PlayerRespawn : MonoBehaviour
{
    public Vector3 spawnPoint;
    public GameObject deathParticlesPrefab; 
    private PlayerController playerController;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Die()
    {
        if (deathParticlesPrefab != null)
        {
            GameObject particles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 2f);
        }
        if (playerController != null)
            playerController.enabled = false;
        if (rb != null)
            rb.simulated = false;
        if (boxCollider != null)
            boxCollider.enabled = false;
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.enabled = false;
        if (animator != null)
            animator.enabled = false;
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        if (playerController != null)
            playerController.enabled = true;
        if (rb != null)
            rb.simulated = true;
        if (boxCollider != null)
            boxCollider.enabled = true;
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.enabled = true;
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Spawn", 0, 0f);
        }
    }
}
