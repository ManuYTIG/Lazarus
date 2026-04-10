using UnityEngine;

public class RockBreakScript : MonoBehaviour
{
    public Collider2D collider;
    public GameObject particlePrefab;

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Boss")) {
            Destroy(gameObject);
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
}
