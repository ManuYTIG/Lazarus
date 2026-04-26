using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Health health;
    public GameObject healthBar;
    public GameObject healthBarDelta;
    public GameObject border;
    private float width;
    private float left;
    private RectTransform rt;
    private RectTransform rtDelta;
    private float prevDeltaHealth;
    private float currentRatioDelta = 1f;
    private float prevHealth;
    private float prevTime = 0f;
    private float deltaDuration = 0.5f;
    private float timeBuffer = 0.1f;
    private float offset = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthBar.TryGetComponent<RectTransform>(out rt)) {
            Debug.Log("set Rect Transform");
            width = rt.rect.width;
        }

        prevHealth = health.health;
        prevDeltaHealth = health.health;
        prevTime = Time.time;

        healthBarDelta.TryGetComponent<RectTransform>(out rtDelta);
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = (float)(health.health) / health.maxHealth;
        
            
        if (health.health < prevHealth) {
            prevDeltaHealth = currentRatioDelta * health.maxHealth;
            prevHealth = health.health;
            prevTime = Time.time;
        } else if (health.health > prevHealth) {
            currentRatioDelta = ratio;
            prevDeltaHealth = health.health;
            prevHealth = health.health;
        }

        if (currentRatioDelta != (float)(health.health) / health.maxHealth && (Time.time - prevTime) > timeBuffer)
        {
            currentRatioDelta = Mathf.Lerp(prevDeltaHealth / health.maxHealth,
                ratio, (Time.time - prevTime - timeBuffer) / deltaDuration);
        }


        rt.offsetMax = new Vector2((border.GetComponent<RectTransform>().rect.width) * (ratio - 1) - offset, rt.offsetMax.y);
        rtDelta.offsetMax = new Vector2((border.GetComponent<RectTransform>().rect.width) * (currentRatioDelta - 1) - offset, rt.offsetMax.y);
    }
}
