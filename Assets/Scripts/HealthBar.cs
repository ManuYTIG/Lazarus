using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Health health;
    public GameObject healthBar;
    private float width;
    private float left;
    private RectTransform rt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthBar.TryGetComponent<RectTransform>(out rt)) {
            Debug.Log("set Rect Transform");
            width = rt.rect.width;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = (float)(health.health)/health.maxHealth;

        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 400, width * ratio);
    }
}
