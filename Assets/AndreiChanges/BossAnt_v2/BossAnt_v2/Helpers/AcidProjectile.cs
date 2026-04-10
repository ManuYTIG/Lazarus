using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public GameObject poolPrefab;
    [HideInInspector] public float poolDuration;
    [HideInInspector] public float poolDamagePerSec;
    public float lifeTime    = 1.5f;
    public float particleTime;

    private bool _splashed   = false;
    private float _spawnImmunity = 0.15f; // ignore collisions for first 0.15s
    public GameObject splashParticle;

    void Update()
    {
        if (_spawnImmunity > 0f)
        {
            _spawnImmunity -= Time.deltaTime;
            return; // don't tick lifetime during immunity
        }

        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) SplashPool();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_spawnImmunity > 0f) return;           // ← ignore early collisions
        if (col.gameObject.CompareTag("Boss")) return;
        if (col.gameObject.CompareTag("Minion")) return;

        SplashPool();
    }

    void SplashPool()
    {
        if (_splashed) return;
        _splashed = true;

        if (poolPrefab != null)
        {
            GameObject pool = Instantiate(poolPrefab, transform.position, Quaternion.identity);
        
            float randomScale = Random.Range(1f, 3f);
            pool.transform.localScale = Vector3.one * randomScale; // ← random scale 1–3

            AcidPool ap = pool.GetComponent<AcidPool>();
            if (ap != null) ap.duration = poolDuration;
            SpawnParticle();
        }

        Destroy(gameObject);
    }
    private void SpawnParticle()
    {
        if (splashParticle != null) {
            GameObject p = Instantiate(splashParticle, transform.position, Quaternion.identity);
            Destroy(p, particleTime);
        }
        
    }
}