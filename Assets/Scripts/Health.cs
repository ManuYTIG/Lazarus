using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveHealth(float amount)
    {
        health -= amount;
        StartCoroutine(DamageFlash());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        Color original = spriteRenderer.color;

        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f); // flash red
        yield return new WaitForSeconds(0.1f);  // wait 0.1 seconds
        spriteRenderer.color = original;                    // restore
    }

}






