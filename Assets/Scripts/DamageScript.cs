using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] private string tagImmune = "Player"; // Default to Player
    [SerializeField] private float damageValue;
    [SerializeField] private bool isProjectile;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // 1. Check for immunity first
        if (collision.gameObject.CompareTag(tagImmune)) return;

        // 2. Try to get the health component efficiently
        if (collision.gameObject.TryGetComponent(out Health targetHealth)) 
        {
            targetHealth.RemoveHealth(damageValue);
        }

        // 3. Handle projectile destruction
        if (isProjectile) 
        {
            // This destroys the whole object (the bullet), not just the script
            Destroy(gameObject); 
        }
    }
}