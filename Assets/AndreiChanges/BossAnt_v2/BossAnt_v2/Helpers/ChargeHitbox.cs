using System.Collections;
using UnityEngine;

public class ChargeHitbox : MonoBehaviour
{
    private Collider2D col;
    private float      damage;
    private void Awake() => col = GetComponent<Collider2D>();

    public void Activate(float dmg, float duration)
    {
        damage = dmg;
        if (col != null) col.enabled = true;
        StartCoroutine(Deactivate(duration));
    }

    private IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null) col.enabled = false;
    }

    // Utilise Health.RemoveHealth
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null) h.RemoveHealth(damage);
    }
}
