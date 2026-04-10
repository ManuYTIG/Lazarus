using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    [HideInInspector] public GameObject poolPrefab;
    [HideInInspector] public float      poolDuration;
    [HideInInspector] public float      poolDamagePerSec;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (poolPrefab != null)
        {
            GameObject pool = Instantiate(poolPrefab, transform.position, Quaternion.identity);
            AcidPool   ap   = pool.GetComponent<AcidPool>();
            if (ap != null) { ap.duration = poolDuration;}
        }
        Destroy(gameObject);
    }
}
