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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health=maxHealth;
        healthBar = Instantiate(healthBarPrefab, transform).transform;
        
        // Position it above the enemy
        healthBar.localPosition = new Vector3(0, 1.2f, 0);

        // Get the fill image
        fillImage = healthBar.Find("Fill").GetComponent<Image>();

        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateHealthBar()
    {
        fillImage.fillAmount = (health / maxHealth)%1;
    }

    public void RemoveHealth(float amount)
    {
        health -= amount;
        UpdateHealthBar();

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(DamageFlash());

        if (health <= 0)
            Destroy(gameObject);
    }

    private IEnumerator DamageFlash()
    {
        Color original = Color.white;

        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = original;
    }

}






