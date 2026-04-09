using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barre de vie du boss avec changement de couleur par phase.
/// Compatible avec le composant Health existant.
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    [Header("UI")]
    public Slider mainSlider;
    public Image  fillImage;

    [Header("Couleurs")]
    public Color phase1Color = new Color(0.2f, 0.8f, 0.3f); // vert
    public Color phase2Color = new Color(1.0f, 0.6f, 0.0f); // orange
    public Color phase3Color = new Color(0.9f, 0.1f, 0.1f); // rouge

    private Health bossHealth;

    private void Start()
    {
        bossHealth = GetComponent<Health>();
        if (mainSlider != null)
        {
            mainSlider.maxValue = bossHealth.maxHealth;
            mainSlider.value    = bossHealth.maxHealth;
        }
        SetColor(phase1Color);
    }

    private void Update()
    {
        if (bossHealth == null) return;

        if (mainSlider != null)
            mainSlider.value = bossHealth.health;

        float ratio = bossHealth.health / bossHealth.maxHealth;
        if      (ratio > 0.66f) SetColor(phase1Color);
        else if (ratio > 0.33f) SetColor(phase2Color);
        else                    SetColor(phase3Color);
    }

    private void SetColor(Color c) { if (fillImage != null) fillImage.color = c; }
}
