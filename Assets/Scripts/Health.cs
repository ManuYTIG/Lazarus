using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public SpriteRenderer spriteRenderer;
    public GameObject healthBarPrefab;
    private Image fillImage;
    private Transform healthBar;
    private Coroutine flashRoutine;
    private PlayerRespawn playerRespawn; // <-- added

    void Start()
    {
        health = maxHealth;
        healthBar = Instantiate(healthBarPrefab, transform).transform;
        healthBar.localPosition = new Vector3(0, 1.2f, 0);
        fillImage = healthBar.Find("Fill").GetComponent<Image>();
        UpdateHealthBar();
        playerRespawn = GetComponent<PlayerRespawn>(); // <-- added
    }
    public void ResetHealth()
    {
        health = maxHealth;
        UpdateHealthBar();
    }
    void Update()
    {
        
    }

    void UpdateHealthBar()
    {
        fillImage.fillAmount = (health / maxHealth) % 1;
    }

    public void RemoveHealth(float amount)
    {
        health -= amount;
        UpdateHealthBar();
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(DamageFlash());
        if (health <= 0)
        {
            if (playerRespawn != null)
                playerRespawn.Die(); // <-- calls Die() instead of Destroy
            else
                Destroy(gameObject); // fallback if no PlayerRespawn exists
        }
    }

    private IEnumerator DamageFlash()
    {
        Color original = Color.white;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = original;
    }
}






