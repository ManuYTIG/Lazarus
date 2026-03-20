using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;

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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color original = sr.color;

        sr.color = new Color(1f, 0f, 0f, 0.5f); // flash red
        yield return new WaitForSeconds(0.1f);  // wait 0.1 seconds
        sr.color = original;                    // restore
    }

}






