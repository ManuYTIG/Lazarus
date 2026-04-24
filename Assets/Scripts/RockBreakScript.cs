using UnityEngine;
using System.Collections;

public class RockBreakScript : MonoBehaviour
{
    public Collider2D collider;
    public GameObject particlePrefab;
    public float damages = 30f;
    public float particleTime = 2f;

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Boss")) {
            if(col.gameObject.TryGetComponent(out Health health)) {
                health.RemoveHealth(damages);
            }
            Destroy(gameObject);
            GameObject p = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(p, particleTime);
            StartCoroutine(ScreenShake(0.5f, 1f)); 
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Camera  cam         = Camera.main;
        Vector3 originalPos = cam.transform.localPosition;
        float   elapsed     = 0f;

        while (elapsed < duration)
        {
            cam.transform.localPosition = originalPos + new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = originalPos;
    }
}
