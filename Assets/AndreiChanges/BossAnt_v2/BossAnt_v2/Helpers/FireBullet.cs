using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float damage = 18f;

    // Utilise Health.RemoveHealth
    private void OnTriggerEnter2D(Collider2D col)
    {
        Health h = col.GetComponent<Health>();
        if (h != null) h.RemoveHealth(damage);
    }
}
