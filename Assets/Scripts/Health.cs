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
    public void RemoveHealth(float amount) {
        health-=amount;
        if(health<=0){
            Destroy(gameObject);
        }
    }
}






